using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly List<(string Username, string Password, string Role)> _users = new()
        {
            ("admin", "123", "Admin"),
            ("user", "456", "User")
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == default)
                return Unauthorized("Invalid username or password.");
						if (string.IsNullOrEmpty(user.Role))
								return Unauthorized("Role not assigned.");
								
            var token = GenerateJwtToken(user.Username, user.Role);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Iss, jwtSettings["Issuer"]), // Add the Issuer claim
                new Claim(JwtRegisteredClaimNames.Aud, jwtSettings["Audience"]) // Add the Audience claim
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
    
    

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}

