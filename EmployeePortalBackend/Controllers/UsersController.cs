using EmployeePortalBackend.Data;
using EmployeePortalBackend.Dto;
using EmployeePortalBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Task<T> - Asynchronous Operation
        // ActionResult<T> - ASP.NET Core Web API Return Type
        // IEnumerable<UserDto> - Collection Interface
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role) // Eager loading - loads Role data with Users
                .Select(u => new UserResponseDto // Create new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Role = u.Role,
                })
                .ToListAsync();

            return Ok(users); // Retrun UserDtos
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Check if username exists
                if (await _context.Users.AnyAsync(u => u.UserName == userRequestDto.UserName))
                    return BadRequest("Username already exists");

                // Create new User Object
                var user = new User
                {
                    UserName = userRequestDto.UserName,
                    Password = userRequestDto.Password,
                    RoleId = userRequestDto.RoleId,
                };

                // Save to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user : " + ex.Message);
            }
        }
    }
}
