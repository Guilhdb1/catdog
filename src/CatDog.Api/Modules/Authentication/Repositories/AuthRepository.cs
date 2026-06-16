using CatDog.Api.Data;
using CatDog.Api.Modules.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatDog.Api.Modules.Authentication.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AuthRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<ConfirmationToken?> FindConfirmationTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ConfirmationTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }

    public async Task<PasswordResetToken?> FindPasswordResetTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }

    public async Task<RefreshToken?> FindRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }

    public async Task<IReadOnlyList<RefreshToken>> GetActiveRefreshTokensByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Where(x => x.UserId == userId && !x.Revoked)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task AddConfirmationTokenAsync(ConfirmationToken token, CancellationToken cancellationToken = default)
    {
        await _dbContext.ConfirmationTokens.AddAsync(token, cancellationToken);
    }

    public async Task AddPasswordResetTokenAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        await _dbContext.PasswordResetTokens.AddAsync(token, cancellationToken);
    }

    public Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string? reason = null, CancellationToken cancellationToken = default)
    {
        refreshToken.Revoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.ReplacedByTokenHash = reason;
        return Task.CompletedTask;
    }

    public async Task RevokeAllRefreshTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _dbContext.RefreshTokens.Where(x => x.UserId == userId && !x.Revoked).ToListAsync(cancellationToken);
        foreach (var token in tokens)
        {
            token.Revoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
    }
}
