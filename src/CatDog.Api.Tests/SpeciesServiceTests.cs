using System;
using System.Threading;
using System.Threading.Tasks;
using CatDog.Api.Data;
using CatDog.Api.Modules.SpeciesManagement.DTOs;
using CatDog.Api.Modules.SpeciesManagement.Entities;
using CatDog.Api.Modules.SpeciesManagement.Repositories;
using CatDog.Api.Modules.SpeciesManagement.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatDog.Api.Tests;

public class SpeciesServiceTests
{
    // CT-01: Nome válido → cadastro com sucesso
    [Fact]
    public async Task ExecuteAsync_ValidName_CreatesSpecies()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None);

        Assert.Equal("Cachorro", response.Name);
        Assert.Equal("Espécie cadastrada com sucesso.", response.Message);
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    // CT-02: Nome no limite mínimo (2 chars)
    [Fact]
    public async Task ExecuteAsync_NameAtMinLength_Succeeds()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Gá" }, CancellationToken.None);

        Assert.Equal("Gá", response.Name);
    }

    // CT-03: Nome no limite máximo (50 chars)
    [Fact]
    public async Task ExecuteAsync_NameAtMaxLength_Succeeds()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        var name = new string('A', 50);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = name }, CancellationToken.None);

        Assert.Equal(name, response.Name);
    }

    // CT-04: Nome com espaços nas extremidades → trim aplicado
    [Fact]
    public async Task ExecuteAsync_NameWithWhitespacePadding_StoresTrimmedName()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = " Coelho " }, CancellationToken.None);

        Assert.Equal("Coelho", response.Name);
    }

    // CT-05: Nome vazio → erro obrigatório
    [Fact]
    public async Task ExecuteAsync_EmptyName_ThrowsRequiredError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "" }, CancellationToken.None));

        Assert.Equal("O nome da espécie é obrigatório.", ex.Message);
    }

    // CT-06: Nome com apenas espaços → erro obrigatório
    [Fact]
    public async Task ExecuteAsync_WhitespaceOnlyName_ThrowsRequiredError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "   " }, CancellationToken.None));

        Assert.Equal("O nome da espécie é obrigatório.", ex.Message);
    }

    // CT-07: Nome nulo → erro obrigatório
    [Fact]
    public async Task ExecuteAsync_NullName_ThrowsRequiredError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = null }, CancellationToken.None));

        Assert.Equal("O nome da espécie é obrigatório.", ex.Message);
    }

    // CT-07 (spec): Nome com 1 caractere após trim → erro mínimo
    [Fact]
    public async Task ExecuteAsync_NameBelowMinLength_ThrowsMinLengthError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "G" }, CancellationToken.None));

        Assert.Equal("O nome da espécie deve ter no mínimo 2 caracteres.", ex.Message);
    }

    // CT-08: Nome com 51 caracteres → erro máximo
    [Fact]
    public async Task ExecuteAsync_NameAboveMaxLength_ThrowsMaxLengthError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        var name = new string('A', 51);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = name }, CancellationToken.None));

        Assert.Equal("O nome da espécie deve ter no máximo 50 caracteres.", ex.Message);
    }

    // CT-09: Nome duplicado exato → erro duplicado
    [Fact]
    public async Task ExecuteAsync_ExactDuplicateName_ThrowsDuplicateError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None));

        Assert.Equal("Já existe uma espécie com este nome.", ex.Message);
    }

    // CT-10: Nome duplicado em caixa baixa → erro duplicado (case-insensitive)
    [Fact]
    public async Task ExecuteAsync_LowercaseDuplicateName_ThrowsDuplicateError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "cachorro" }, CancellationToken.None));

        Assert.Equal("Já existe uma espécie com este nome.", ex.Message);
    }

    // CT-11: Nome duplicado em caixa alta → erro duplicado (case-insensitive)
    [Fact]
    public async Task ExecuteAsync_UppercaseDuplicateName_ThrowsDuplicateError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "CACHORRO" }, CancellationToken.None));

        Assert.Equal("Já existe uma espécie com este nome.", ex.Message);
    }

    // CT-14: Nome com 2 chars após trim
    [Fact]
    public async Task ExecuteAsync_NameTwoCharsAfterTrim_Succeeds()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = " Ab " }, CancellationToken.None);

        Assert.Equal("Ab", response.Name);
    }

    // CT-15: Nome com caracteres especiais → sucesso
    [Fact]
    public async Task ExecuteAsync_NameWithSpecialCharacters_Succeeds()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Ave-Exótica" }, CancellationToken.None);

        Assert.Equal("Ave-Exótica", response.Name);
    }

    // CT-16: Dois cadastros válidos sequenciais → ambas registradas
    [Fact]
    public async Task ExecuteAsync_TwoSequentialValidRegistrations_BothSucceed()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);

        var response1 = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Cachorro" }, CancellationToken.None);
        var response2 = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Gato" }, CancellationToken.None);

        Assert.Equal("Cachorro", response1.Name);
        Assert.Equal("Gato", response2.Name);
        Assert.Equal(2, await context.Species.CountAsync());
    }

    // Validação: resposta inclui CreatedAt com valor recente
    [Fact]
    public async Task ExecuteAsync_ValidName_ResponseContainsCreatedAt()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        var before = DateTime.UtcNow.AddSeconds(-1);

        var response = await useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = "Hamster" }, CancellationToken.None);

        Assert.True(response.CreatedAt >= before);
    }

    // Validação: nome com exatamente 51 chars após trim → erro máximo (confirma que trim não ajuda contra max)
    [Fact]
    public async Task ExecuteAsync_NameWith51CharsAfterTrim_ThrowsMaxLengthError()
    {
        using var context = CreateContext();
        var useCase = CreateUseCase(context);
        var name = " " + new string('B', 51) + " ";

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(new RegisterSpeciesRequest { Name = name }, CancellationToken.None));

        Assert.Equal("O nome da espécie deve ter no máximo 50 caracteres.", ex.Message);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static RegisterSpeciesUseCase CreateUseCase(ApplicationDbContext context)
    {
        var repository = new SpeciesRepository(context);
        return new RegisterSpeciesUseCase(repository);
    }
}
