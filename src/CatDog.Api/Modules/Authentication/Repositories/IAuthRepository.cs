using CatDog.Api.Modules.Authentication.Entities;

namespace CatDog.Api.Modules.Authentication.Repositories;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<ConfirmationToken?> FindConfirmationTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<PasswordResetToken?> FindPasswordResetTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<RefreshToken?> FindRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RefreshToken>> GetActiveRefreshTokensByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task AddConfirmationTokenAsync(ConfirmationToken token, CancellationToken cancellationToken = default);
    Task AddPasswordResetTokenAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string? reason = null, CancellationToken cancellationToken = default);
    Task RevokeAllRefreshTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
