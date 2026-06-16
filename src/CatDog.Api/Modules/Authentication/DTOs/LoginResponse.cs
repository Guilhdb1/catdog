namespace CatDog.Api.Modules.Authentication.DTOs;

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public UserDto User { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string RedirectTo { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
}
