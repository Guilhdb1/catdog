using System.IdentityModel.Tokens.Jwt;
using CatDog.Api.Modules.Authentication.DTOs;
using CatDog.Api.Modules.Authentication.Services;
using CatDog.Api.Modules.Authentication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatDog.Api.Modules.Authentication.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private const string RefreshCookieName = "refresh_token";
    private readonly AuthService _authService;
    private readonly TokenManager _tokenManager;

    public AuthController(AuthService authService, TokenManager tokenManager)
    {
        _authService = authService;
        _tokenManager = tokenManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var baseUrl = GetBaseUrl();
            await _authService.RegisterAsync(request, baseUrl, cancellationToken);
            return Created(string.Empty, new { message = "Cadastro realizado. Verifique seu e-mail para confirmar a conta." });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Email já cadastrado"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ConfirmEmailAsync(token, cancellationToken);
            return Ok(new { message = "E-mail confirmado com sucesso." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            var (response, refreshTokenValue) = await _authService.LoginAsync(request, userAgent, cancellationToken);
            Response.Cookies.Append(RefreshCookieName, refreshTokenValue, _tokenManager.CreateRefreshCookieOptions());
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("bloqueada"))
            {
                return StatusCode(423, new { error = ex.Message });
            }

            if (ex.Message.Contains("Confirme seu e-mail"))
            {
                return StatusCode(403, new { error = ex.Message });
            }

            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(RefreshCookieName, out var currentToken))
        {
            return Unauthorized(new { error = "Refresh token não fornecido." });
        }

        try
        {
            var (newRefreshToken, rawToken) = await _authService.RefreshAsync(currentToken, cancellationToken);
            var user = newRefreshToken.User;
            var accessToken = _tokenManager.CreateAccessToken(user);
            Response.Cookies.Append(RefreshCookieName, rawToken, _tokenManager.CreateRefreshCookieOptions());
            return Ok(new { access_token = accessToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            if (ex.Message.Contains("Reuse"))
            {
                return StatusCode(403, new { error = ex.Message });
            }

            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken))
        {
            await _authService.LogoutAsync(refreshToken, cancellationToken);
        }

        Response.Cookies.Append(RefreshCookieName, string.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            Path = "/"
        });

        return Ok(new { message = "Logout realizado com sucesso." });
    }

    [HttpPost("forgot")]
    public async Task<IActionResult> Forgot([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = GetBaseUrl();
        await _authService.ForgotPasswordAsync(request, baseUrl, cancellationToken);
        return Ok(new { message = "Se o e-mail existir, você receberá instruções para redefinir sua senha." });
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ResetPasswordAsync(request, cancellationToken);
            return Ok(new { message = "Senha redefinida com sucesso." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out var id))
        {
            return Unauthorized(new { error = "Token inválido." });
        }

        var profile = await _authService.GetProfileAsync(id, cancellationToken);
        return Ok(profile);
    }

    private string GetBaseUrl()
    {
        var request = HttpContext.Request;
        return $"{request.Scheme}://{request.Host.Value}";
    }
}
