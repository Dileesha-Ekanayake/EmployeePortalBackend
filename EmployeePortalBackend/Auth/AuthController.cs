using EmployeePortalBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeePortalBackend.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LogIn([FromBody] LogInRequestDto logInRequest)
        {
            // Find the use by UserName and Password
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(
                    u => u.UserName == logInRequest.UserName &&
                    u.Password == logInRequest.Password
                );

            if (user == null)
                return Unauthorized("Invalid Credentials");

            // Create a symmetric security key from the secret configured in appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Create signing credentials using the security key and HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Build the claims that will be embedded into the JWT payload
            var claims = new[]
            {
                // object identifier
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Insted of "ClaimTypes.NameIdentifier" can use simple values likes -> uid, name, role
                // Username
                new Claim(ClaimTypes.Name, user.UserName),
                // Role claim — used by ASP.NET Core [Authorize(Roles = "...")] checks Role based authorization
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            // Create the token object with issuer, audience, claims, expiry and signing credentials
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],               // expected token issuer
                audience: _config["Jwt:Audience"],           // expected token audience
                claims: claims,                              // claims to include in the token payload
                expires: DateTime.Now.AddHours(1),           // token expiration time
                signingCredentials: credentials              // cryptographic signature info
            );

            // Serialize the token object to a compact JWT string and return it in the response DTO
            return Ok(new TokenResponse { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
