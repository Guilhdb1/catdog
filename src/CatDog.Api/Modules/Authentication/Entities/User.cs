using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatDog.Api.Modules.Authentication.Entities;

public enum UserRole
{
    ADMIN,
    ADOTANTE
}

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(180)]
    public string Nome { get; set; } = null!;

    [Required]
    [MaxLength(320)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    public bool EmailConfirmed { get; set; }
    public DateTime? EmailConfirmedAt { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.ADOTANTE;

    public int FailedLoginAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<ConfirmationToken> ConfirmationTokens { get; set; } = new List<ConfirmationToken>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
}
