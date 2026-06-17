using CatDog.Api.Modules.SpeciesManagement.DTOs;
using CatDog.Api.Modules.SpeciesManagement.Entities;
using CatDog.Api.Modules.SpeciesManagement.Repositories;

namespace CatDog.Api.Modules.SpeciesManagement.Services;

public class RegisterSpeciesUseCase
{
    private readonly ISpeciesRepository _repository;

    public RegisterSpeciesUseCase(ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegisterSpeciesResponse> ExecuteAsync(RegisterSpeciesRequest request, CancellationToken cancellationToken = default)
    {
        var trimmedName = request.Name?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(trimmedName))
        {
            throw new InvalidOperationException("O nome da espécie é obrigatório.");
        }

        if (trimmedName.Length < 2)
        {
            throw new InvalidOperationException("O nome da espécie deve ter no mínimo 2 caracteres.");
        }

        if (trimmedName.Length > 50)
        {
            throw new InvalidOperationException("O nome da espécie deve ter no máximo 50 caracteres.");
        }

        var alreadyExists = await _repository.ExistsByNameAsync(trimmedName, cancellationToken);
        if (alreadyExists)
        {
            throw new InvalidOperationException("Já existe uma espécie com este nome.");
        }

        var species = new Species
        {
            Name = trimmedName,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(species, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new RegisterSpeciesResponse
        {
            Id = species.Id,
            Name = species.Name,
            CreatedAt = species.CreatedAt,
            Message = "Espécie cadastrada com sucesso."
        };
    }
}
