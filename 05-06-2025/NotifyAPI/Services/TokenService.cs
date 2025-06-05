using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotifyAPI.Models;
using NotifyAPI.Interfaces;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _securityKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration configuration)
    {
        var secret = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
            throw new InvalidOperationException("JWT SecretKey is missing or too short (min 256 bits = 32 bytes)");

        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _issuer = configuration["Jwt:Issuer"]!;
        _audience = configuration["Jwt:Audience"]!;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
