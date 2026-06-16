using System.ComponentModel.DataAnnotations;

namespace CatDog.Api.Modules.Authentication.DTOs;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
