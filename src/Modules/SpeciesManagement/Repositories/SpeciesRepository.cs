using CatDog.Api.Data;
using CatDog.Api.Modules.SpeciesManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatDog.Api.Modules.SpeciesManagement.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SpeciesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.ToLowerInvariant();
        return await _dbContext.Species
            .AnyAsync(x => x.Name.ToLower() == normalizedName, cancellationToken);
    }

    public async Task AddAsync(Species species, CancellationToken cancellationToken = default)
    {
        await _dbContext.Species.AddAsync(species, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
