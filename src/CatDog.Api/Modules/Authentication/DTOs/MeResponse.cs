namespace CatDog.Api.Modules.Authentication.DTOs;

public class MeResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}
