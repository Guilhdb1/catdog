using System.Text.RegularExpressions;
using CatDog.Api.Modules.Authentication.DTOs;
using CatDog.Api.Modules.Authentication.Entities;
using CatDog.Api.Modules.Authentication.Infrastructure;
using CatDog.Api.Modules.Authentication.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CatDog.Api.Modules.Authentication.Services;

public class AuthService
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutPeriod = TimeSpan.FromMinutes(15);
    private static readonly IReadOnlySet<string> BlockedPasswords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "123456789",
        "password",
        "password123",
        "qwerty",
        "qwerty123",
        "12345678",
        "abc123",
        "letmein"
    };

    private readonly IAuthRepository _repository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IEmailService _emailService;
    private readonly TokenManager _tokenManager;

    public AuthService(IAuthRepository repository, IPasswordHasher<User> hasher, IEmailService emailService, TokenManager tokenManager)
    {
        _repository = repository;
        _hasher = hasher;
        _emailService = emailService;
        _tokenManager = tokenManager;
    }

    public async Task RegisterAsync(RegisterRequest request, string confirmationBaseUrl, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _repository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email já cadastrado.");
        }

        ValidatePassword(request.Nome, normalizedEmail, request.Senha);

        var user = new User
        {
            Nome = request.Nome.Trim(),
            Email = normalizedEmail,
            PasswordHash = _hasher.HashPassword(null!, request.Senha),
            Role = UserRole.ADOTANTE,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddUserAsync(user, cancellationToken);

        var tokenString = _tokenManager.CreateRefreshTokenString();
        var confirmationToken = new ConfirmationToken
        {
            UserId = user.Id,
            TokenHash = _tokenManager.HashToken(tokenString),
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        await _repository.AddConfirmationTokenAsync(confirmationToken, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var confirmationUrl = $"{confirmationBaseUrl.TrimEnd('/')}/auth/confirm?token={Uri.EscapeDataString(tokenString)}";
        await _emailService.SendConfirmationEmailAsync(user.Email, user.Nome, confirmationUrl, cancellationToken);
    }

    public async Task ConfirmEmailAsync(string token, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenManager.HashToken(token);
        var confirmationToken = await _repository.FindConfirmationTokenAsync(tokenHash, cancellationToken);
        if (confirmationToken is null || confirmationToken.UsedAt is not null || confirmationToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Token de confirmação inválido ou expirado.");
        }

        confirmationToken.UsedAt = DateTime.UtcNow;
        confirmationToken.User.EmailConfirmed = true;
        confirmationToken.User.EmailConfirmedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<(LoginResponse Response, string RefreshTokenValue)> LoginAsync(LoginRequest request, string? deviceInfo, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _repository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        if (user.LockedUntil is not null && user.LockedUntil > DateTime.UtcNow)
        {
            throw new InvalidOperationException("Conta bloqueada temporariamente. Tente novamente mais tarde.");
        }

        if (!user.EmailConfirmed)
        {
            throw new InvalidOperationException("Confirme seu e-mail antes de efetuar login.");
        }

        var verificationResult = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Senha);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            user.FailedLoginAttempts += 1;
            if (user.FailedLoginAttempts >= MaxFailedAttempts)
            {
                user.LockedUntil = DateTime.UtcNow.Add(LockoutPeriod);
            }

            await _repository.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        var accessToken = _tokenManager.CreateAccessToken(user);
        var refreshTokenValue = _tokenManager.CreateRefreshTokenString();
        var refreshToken = _tokenManager.BuildRefreshToken(user, refreshTokenValue, deviceInfo);

        await _repository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return (
            new LoginResponse
            {
                AccessToken = accessToken,
                Role = user.Role.ToString(),
                RedirectTo = user.Role == UserRole.ADMIN ? "/admin/dashboard" : "/adotante/home",
                User = new UserDto
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email
                }
            },
            refreshTokenValue);
    }

    public async Task<MeResponse> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Usuário não encontrado.");
        }

        return new MeResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<(RefreshToken RefreshToken, string RawToken)> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshHash = _tokenManager.HashToken(refreshToken);
        var currentToken = await _repository.FindRefreshTokenAsync(refreshHash, cancellationToken);
        if (currentToken is null)
        {
            throw new UnauthorizedAccessException("Refresh token inválido.");
        }

        if (currentToken.Revoked || currentToken.ExpiresAt < DateTime.UtcNow)
        {
            if (currentToken.Revoked)
            {
                await _repository.RevokeAllRefreshTokensForUserAsync(currentToken.UserId, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
                throw new UnauthorizedAccessException("Reuse de refresh token detectado. Refaça login.");
            }

            throw new UnauthorizedAccessException("Refresh token expirado.");
        }

        currentToken.Revoked = true;
        currentToken.RevokedAt = DateTime.UtcNow;
        currentToken.LastUsedAt = DateTime.UtcNow;

        var newRefreshValue = _tokenManager.CreateRefreshTokenString();
        var newRefreshToken = _tokenManager.BuildRefreshToken(currentToken.User, newRefreshValue, currentToken.DeviceInfo);
        currentToken.ReplacedByTokenHash = newRefreshToken.TokenHash;

        await _repository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return (newRefreshToken, newRefreshValue);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshHash = _tokenManager.HashToken(refreshToken);
        var currentToken = await _repository.FindRefreshTokenAsync(refreshHash, cancellationToken);
        if (currentToken is null)
        {
            return;
        }

        await _repository.RevokeRefreshTokenAsync(currentToken, "logout", cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string resetBaseUrl, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _repository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null)
        {
            return;
        }

        var resetTokenValue = _tokenManager.CreateRefreshTokenString();
        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            TokenHash = _tokenManager.HashToken(resetTokenValue),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        await _repository.AddPasswordResetTokenAsync(resetToken, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var resetUrl = $"{resetBaseUrl.TrimEnd('/')}/auth/reset-password?token={Uri.EscapeDataString(resetTokenValue)}";
        await _emailService.SendPasswordResetEmailAsync(user.Email, user.Nome, resetUrl, cancellationToken);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenManager.HashToken(request.Token);
        var resetToken = await _repository.FindPasswordResetTokenAsync(tokenHash, cancellationToken);
        if (resetToken is null || resetToken.UsedAt is not null || resetToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Token de redefinição inválido ou expirado.");
        }

        var user = resetToken.User;
        ValidatePassword(user.Nome, user.Email, request.Senha);

        user.PasswordHash = _hasher.HashPassword(user, request.Senha);
        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        user.UpdatedAt = DateTime.UtcNow;
        resetToken.UsedAt = DateTime.UtcNow;

        await _repository.RevokeAllRefreshTokensForUserAsync(user.Id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static void ValidatePassword(string name, string email, string password)
    {
        if (password.Length < 10)
        {
            throw new InvalidOperationException("A senha deve ter pelo menos 10 caracteres.");
        }

        if (!Regex.IsMatch(password, "[A-Z]"))
        {
            throw new InvalidOperationException("A senha deve conter ao menos uma letra maiúscula.");
        }

        if (!Regex.IsMatch(password, "[a-z]"))
        {
            throw new InvalidOperationException("A senha deve conter ao menos uma letra minúscula.");
        }

        if (!Regex.IsMatch(password, "[0-9]"))
        {
            throw new InvalidOperationException("A senha deve conter ao menos um número.");
        }

        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
        {
            throw new InvalidOperationException("A senha deve conter ao menos um caractere especial.");
        }

        var normalized = password.ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(name) && normalized.Contains(name.Trim().ToLowerInvariant()))
        {
            throw new InvalidOperationException("A senha não pode conter o nome do usuário.");
        }

        if (!string.IsNullOrWhiteSpace(email) && normalized.Contains(email.Trim().ToLowerInvariant()))
        {
            throw new InvalidOperationException("A senha não pode conter o email do usuário.");
        }

        if (BlockedPasswords.Contains(password))
        {
            throw new InvalidOperationException("A senha é muito fraca ou comum.");
        }
    }
}
