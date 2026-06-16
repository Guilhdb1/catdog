using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CatDog.Api.Modules.Authentication.Entities;

namespace CatDog.Api.Modules.Authentication.Infrastructure;

public class TokenManager
{
    private readonly string _jwtSecret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _refreshTokenValidity = TimeSpan.FromDays(30);

    public TokenManager(string jwtSecret, string issuer, string audience)
    {
        _jwtSecret = jwtSecret;
        _issuer = issuer;
        _audience = audience;
    }

    public string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role.ToString()),
            new Claim("name", user.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshTokenString()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }

    public RefreshToken BuildRefreshToken(User user, string tokenString, string? deviceInfo = null)
    {
        return new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(tokenString),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(_refreshTokenValidity),
            DeviceInfo = deviceInfo
        };
    }

    public CookieOptions CreateRefreshCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.Add(_refreshTokenValidity)
        };
    }
}
