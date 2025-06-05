using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth;


namespace HospitalManagementAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration)
        {
            var secret = configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
                throw new InvalidOperationException("JWT SecretKey is missing or too short (must be at least 256 bits = 32 bytes)");

            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        {
            return await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
    }
}
