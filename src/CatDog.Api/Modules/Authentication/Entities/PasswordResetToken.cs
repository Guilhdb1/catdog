using System.ComponentModel.DataAnnotations;

namespace CatDog.Api.Modules.Authentication.Entities;

public class PasswordResetToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string TokenHash { get; set; } = null!;

    [Required]
    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }
    public User User { get; set; } = null!;
}
