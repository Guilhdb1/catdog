using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CatDog.Api.Data;
using CatDog.Api.Modules.Authentication.Entities;
using CatDog.Api.Modules.SpeciesManagement.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace CatDog.Api.Tests;

public class SpeciesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SpeciesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // CT-01: ADMIN cadastra espécie válida → 201 Created
    [Fact]
    public async Task PostSpecies_AdminWithValidName_Returns201()
    {
        var client = _factory.CreateAdminClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = "Cachorro" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<RegisterSpeciesResponse>();
        Assert.NotNull(body);
        Assert.Equal("Cachorro", body!.Name);
        Assert.Equal("Espécie cadastrada com sucesso.", body.Message);
    }

    // CT-05: Nome vazio → 400 Bad Request
    [Fact]
    public async Task PostSpecies_EmptyName_Returns400()
    {
        var client = _factory.CreateAdminClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // CT-09: Nome duplicado → 409 Conflict
    [Fact]
    public async Task PostSpecies_DuplicateName_Returns409()
    {
        var client = _factory.CreateAdminClient();
        await client.PostAsJsonAsync("/api/species", new { name = "Gato-Duplicado" });

        var response = await client.PostAsJsonAsync("/api/species", new { name = "Gato-Duplicado" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    // CT-12: Adotante autenticado → 403 Forbidden
    [Fact]
    public async Task PostSpecies_AdotanteRole_Returns403()
    {
        var client = _factory.CreateAdotanteClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = "Hamster" });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // CT-13: Sem autenticação → 401 Unauthorized
    [Fact]
    public async Task PostSpecies_Unauthenticated_Returns401()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = "Peixe" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // CT-07: Nome com menos de 2 chars → 400 Bad Request
    [Fact]
    public async Task PostSpecies_NameBelowMinLength_Returns400()
    {
        var client = _factory.CreateAdminClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = "G" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // CT-08: Nome com 51 chars → 400 Bad Request
    [Fact]
    public async Task PostSpecies_NameAboveMaxLength_Returns400()
    {
        var client = _factory.CreateAdminClient();
        var response = await client.PostAsJsonAsync("/api/species", new { name = new string('X', 51) });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string JwtSecret = "TestSecretKeyForUnitTestsOnly12345678901234";
    private const string JwtIssuer = "CatDog.Api";
    private const string JwtAudience = "CatDog.Client";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Secret"] = JwtSecret,
                ["JwtSettings:Issuer"] = JwtIssuer,
                ["JwtSettings:Audience"] = JwtAudience,
                ["ConnectionStrings:DefaultConnection"] = $"Data Source=:memory:"
            });
        });

        builder.ConfigureServices(services =>
        {
            var dbDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbDescriptor != null)
            {
                services.Remove(dbDescriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("SpeciesControllerTestsDb");
            });
        });

        builder.UseSetting("JwtSettings:Secret", JwtSecret);
        builder.UseSetting("JwtSettings:Issuer", JwtIssuer);
        builder.UseSetting("JwtSettings:Audience", JwtAudience);
    }

    public HttpClient CreateAdminClient()
    {
        var token = GenerateJwtToken(UserRole.ADMIN);
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public HttpClient CreateAdotanteClient()
    {
        var token = GenerateJwtToken(UserRole.ADOTANTE);
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private string GenerateJwtToken(UserRole role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim("role", role.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, "test@test.com")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
