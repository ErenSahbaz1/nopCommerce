using Microsoft.IdentityModel.Tokens;
using Nop.Core.Domain.Customers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nop.Plugin.Api.Mobile.Services;

/// <summary>
/// JWT token service implementation
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly MobileApiSettings _settings;

    public JwtTokenService(MobileApiSettings settings)
    {
        _settings = settings;
    }

    public Task<string> GenerateTokenAsync(Customer customer)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JwtSecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new(ClaimTypes.Email, customer.Email ?? string.Empty),
            new("customerId", customer.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.JwtExpirationMinutes),
            Issuer = _settings.JwtIssuer,
            Audience = _settings.JwtAudience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<int?> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Task.FromResult<int?>(null);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JwtSecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _settings.JwtIssuer,
                ValidateAudience = true,
                ValidAudience = _settings.JwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "customerId" || x.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                return Task.FromResult<int?>(null);

            if (int.TryParse(claim.Value, out var customerId))
                return Task.FromResult<int?>(customerId);

            return Task.FromResult<int?>(null);
        }
        catch
        {
            return Task.FromResult<int?>(null);
        }
    }
}
