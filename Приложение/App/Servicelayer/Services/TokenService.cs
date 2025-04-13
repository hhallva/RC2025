using DataLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataLayer.Services
{
    public class TokenService(IConfiguration config)
    {
        public string GenerateToken(Employee employee)
        {
            var key = Encoding.UTF8.GetBytes(config["JWT:Key"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, employee.Email),
                new(JwtRegisteredClaimNames.Iss, config["JWT:Issuer"]), 
                new(JwtRegisteredClaimNames.Aud, config["JWT:Audience"])
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(0));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
