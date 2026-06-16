using System.ComponentModel.DataAnnotations;

namespace CatDog.Api.Modules.Authentication.DTOs;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public string Senha { get; set; } = null!;

    [Required]
    [Compare("Senha", ErrorMessage = "A senha e a confirmação devem ser iguais.")]
    public string ConfirmacaoSenha { get; set; } = null!;
}
