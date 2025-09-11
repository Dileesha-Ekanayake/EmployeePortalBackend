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

        // GET: api/dashboard/
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

        // GET: api/dashboard/trending (Posts with >5 likes)
        [HttpGet("trending")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetTrendingPosts()
        {
            var trending = await _context.Posts
                .Include(p => p.Likes) // EAGER load Likes
                .Where(p => p.Likes.Count(l => l.IsLike) > 5) // Filter Posts only have more than 5 likes
                .Select(p => new PostResponseDto // Create new PostResonseDtos
                { 
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    LikeCount = p.Likes.Count(l => l.IsLike) 
                }) 
                .ToListAsync();

            return Ok(trending);
        }
    }
}
