using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Settings;
using LibraryManagementAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementAPI.Services.Services
{
    public class JWTService : IJWTService
    {
        private readonly AuthSettings _authSettings;

        public JWTService(AuthSettings authSettings)
        {
            _authSettings = authSettings;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(_authSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}