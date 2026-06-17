using CatDog.Api.Modules.SpeciesManagement.DTOs;
using CatDog.Api.Modules.SpeciesManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatDog.Api.Modules.SpeciesManagement.Controllers;

[ApiController]
[Route("api/species")]
public class SpeciesController : ControllerBase
{
    private readonly RegisterSpeciesUseCase _useCase;

    public SpeciesController(RegisterSpeciesUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Register([FromBody] RegisterSpeciesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _useCase.ExecuteAsync(request, cancellationToken);
            return Created(string.Empty, response);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Já existe"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
