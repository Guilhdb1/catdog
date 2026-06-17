namespace CatDog.Api.Modules.SpeciesManagement.DTOs;

public class RegisterSpeciesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = null!;
}
