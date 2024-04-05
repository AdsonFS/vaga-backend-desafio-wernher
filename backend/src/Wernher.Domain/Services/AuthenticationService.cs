using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wernher.Domain.Models;

namespace Wernher.Domain.Services;

public class AuthenticationService
{
    private JwtSecurityTokenHandler _tokenHandler;
    private IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _configuration = configuration;
    }

    public bool VerifyPassword(Customer customer, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, customer.Password);
    }

    public string SetHashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public string GenerateToken(Customer customer, bool isAdmin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, customer.Name),
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim("CustomerId", customer.Id.ToString()),

            }, "login"),

            Audience = null,
            Issuer = null,
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = credentials
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}