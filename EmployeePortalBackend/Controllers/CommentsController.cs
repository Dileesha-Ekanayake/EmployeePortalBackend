using EmployeePortalBackend.Data;
using EmployeePortalBackend.Dto;
using EmployeePortalBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/comments?postId=1 (Get comments for a post)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments(int postId)
        {

            // Convert UTC to Local Time (Sri Lanka Standard Time)
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
            var comments = await _context.Comments
                .Where(c => c.PostId == postId) // Filter by Post Id
                .Include(c => c.Author) // EAGER load the Author (User)
                .Select(c => new CommentResponseDto // Create CommentResponseDto (LINQ Query format)
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = TimeZoneInfo.ConvertTimeToUtc(c.CreatedAt, localTimeZone),
                    UserName = c.Author.UserName,
                        UserRole = c.Author.Role.Name
                    })
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/comments (Add comment)
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> AddComment([FromBody] CommentRequestDto commentRequestDto)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Crate new Comment
                var comment = new Comment
                {
                    Content = commentRequestDto.Content,
                    PostId = commentRequestDto.PostId,
                    AuthorId = commentRequestDto.UserId,
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetComments), new { id = comment.Id }, comment);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "An error occurred while adding the comment : " + ex.Message);
            }
        }
    }
}
