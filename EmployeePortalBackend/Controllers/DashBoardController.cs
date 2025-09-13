using EmployeePortalBackend.Data;
using EmployeePortalBackend.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashBoardController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DashboardDto>>> GetMetrics()
        {
            var dashboard = new DashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalPosts = await _context.Posts.CountAsync(),
                TotalComments = await _context.Comments.CountAsync()
            };
            return Ok(dashboard);
        }
    }
}
