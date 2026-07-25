using Domain.Entity;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repository_Implementations.Authentication
{
    public class UserAuthenticate : IUserAuthentication
    {
        private readonly IConfiguration _configuration;

        public UserAuthenticate(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string UserGenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : _configuration["JWT:Issuer"],
                audience : _configuration["JWT:Audience"],
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:ExpiryTime"])),
                signingCredentials : creds
                );
            var handler = new JwtSecurityTokenHandler();

             return handler.WriteToken(token);
        }
    }
}
