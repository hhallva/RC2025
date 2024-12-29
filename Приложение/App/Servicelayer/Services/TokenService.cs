using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servicelayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Servicelayer.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Employee employee)
        {
            var key = Encoding.UTF8.GetBytes(_config["JWT:Key"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, employee.Email),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(30)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
