namespace CatDog.Api.Modules.SpeciesManagement.Entities;

public class Species
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
