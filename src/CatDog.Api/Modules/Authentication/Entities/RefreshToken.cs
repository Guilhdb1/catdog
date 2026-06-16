using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatDog.Api.Modules.Authentication.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string TokenHash { get; set; } = null!;

    [Required]
    public DateTime IssuedAt { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public string? DeviceInfo { get; set; }
    public string? ReplacedByTokenHash { get; set; }

    public User User { get; set; } = null!;
}
