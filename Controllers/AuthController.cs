using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DanfolioBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DanfolioBackend.Controllers;

public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        // Fetch credentials from environment variables
        var storedUsername = Environment.GetEnvironmentVariable("LOGIN_USERNAME")
                             ?? throw new InvalidOperationException("LOGIN_USERNAME environment variable is missing.");;
        var storedPassword = Environment.GetEnvironmentVariable("LOGIN_PASSWORD")
                             ?? throw new InvalidOperationException("LOGIN_PASSWORD environment variable is missing.");;

        // Validate login against environment variables
        if (login.Username == storedUsername && login.Password == storedPassword)
        {
            var token = GenerateJwtToken();
            return Ok(new { token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "user"),
            new Claim(ClaimTypes.Role, "Admin"),
        };
        var issuer  = Environment.GetEnvironmentVariable("JWT_ISSUER")
                      ?? throw new InvalidOperationException("JWT_ISSUER environment variable is missing.");;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") 
                                ?? throw new InvalidOperationException("JWT_KEY environment variable is missing.")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
