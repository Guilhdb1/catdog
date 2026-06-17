using CatDog.Api.Modules.SpeciesManagement.Entities;

namespace CatDog.Api.Modules.SpeciesManagement.Repositories;

public interface ISpeciesRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Species species, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
