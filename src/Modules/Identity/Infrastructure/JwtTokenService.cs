using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineBanking.Modules.Identity.Domain;

namespace OnlineBanking.Modules.Identity.Infrastructure;

public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAtUtc);

public interface IJwtTokenService
{
    TokenResponse CreateTokens(User user);
    string HashRefreshToken(string refreshToken);
}

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;

    public TokenResponse CreateTokens(User user)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_options.AccessTokenMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new TokenResponse(
            new JwtSecurityTokenHandler().WriteToken(jwt),
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            expiresAt);
    }

    public string HashRefreshToken(string refreshToken)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToHexString(hash);
    }
}
