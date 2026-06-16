using System;
using System.Threading;
using System.Threading.Tasks;
using CatDog.Api.Data;
using CatDog.Api.Modules.Authentication.DTOs;
using CatDog.Api.Modules.Authentication.Entities;
using CatDog.Api.Modules.Authentication.Infrastructure;
using CatDog.Api.Modules.Authentication.Repositories;
using CatDog.Api.Modules.Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatDog.Api.Tests;

public class AuthServiceTests
{
    private const string JwtSecret = "TestSecretKeyForUnitTestsOnly12345";
    private const string JwtIssuer = "TestIssuer";
    private const string JwtAudience = "TestAudience";

    [Fact]
    public async Task RegisterAsync_CreatesUserAndSendsEmail()
    {
        using var context = CreateContext();
        var repository = new AuthRepository(context);
        var hasher = new PasswordHasher<User>();
        var emailService = new NoopEmailService();
        var tokenManager = new TokenManager(JwtSecret, JwtIssuer, JwtAudience);
        var service = new AuthService(repository, hasher, emailService, tokenManager);

        var request = new RegisterRequest
        {
            Nome = "Fulano",
            Email = "fulano@example.com",
            Senha = "StrongP@ssw0rd",
            ConfirmacaoSenha = "StrongP@ssw0rd"
        };

        await service.RegisterAsync(request, "https://localhost", CancellationToken.None);
        var created = await context.Users.SingleAsync();

        Assert.Equal("fulano@example.com", created.Email);
        Assert.False(created.EmailConfirmed);
        Assert.Equal(0, created.FailedLoginAttempts);
    }

    [Fact]
    public async Task LoginAsync_WithUnconfirmedEmail_ThrowsInvalidOperationException()
    {
        using var context = CreateContext();
        var user = new User
        {
            Nome = "Fulano",
            Email = "fulano@example.com",
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, "StrongP@ssw0rd"),
            EmailConfirmed = false,
            Role = UserRole.ADOTANTE,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new AuthRepository(context);
        var service = new AuthService(repository, new PasswordHasher<User>(), new NoopEmailService(), new TokenManager(JwtSecret, JwtIssuer, JwtAudience));

        var request = new LoginRequest
        {
            Email = "fulano@example.com",
            Senha = "StrongP@ssw0rd"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoginAsync(request, "test-device", CancellationToken.None));
    }

    [Fact]
    public async Task ResetPasswordAsync_InvalidToken_ThrowsInvalidOperationException()
    {
        using var context = CreateContext();
        var repository = new AuthRepository(context);
        var service = new AuthService(repository, new PasswordHasher<User>(), new NoopEmailService(), new TokenManager(JwtSecret, JwtIssuer, JwtAudience));

        var request = new ResetPasswordRequest
        {
            Token = "invalid-token",
            Senha = "NewP@ssw0rd",
            ConfirmacaoSenha = "NewP@ssw0rd"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ResetPasswordAsync(request, CancellationToken.None));
    }

    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private sealed class NoopEmailService : IEmailService
    {
        public Task SendConfirmationEmailAsync(string email, string name, string confirmationUrl, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetEmailAsync(string email, string name, string resetUrl, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
