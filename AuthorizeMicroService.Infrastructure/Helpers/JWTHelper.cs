using AuthorizeMicroService.Application.Helpers;
using AuthorizeMicroService.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizeMicroService.Infrastructure.Helpers
{
    public class JWTHelper(IOptions<JwtSettings> jwtSettings) : IJwtHelper
    {
        private readonly JwtSettings _jwtSettings = jwtSettings?.Value 
            ?? throw new ArgumentNullException(nameof(jwtSettings));

        public string GenerateJwtToken(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("The user name cannot be empty or contain only spaces.", nameof(userName));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
