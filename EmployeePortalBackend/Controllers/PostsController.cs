using EmployeePortalBackend.Data;
using EmployeePortalBackend.Dto;
using EmployeePortalBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPosts(string? author = null)
        {
            var query = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt) // Latest posts first
                .AsQueryable(); // LINQ (Language Integrated Query)
                                // extension method that converts an object that implements IEnumerable<T> into an IQueryable<T>.

            // Filter by User (Author) name
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(p => p.Author.UserName.Contains(author));
            }
            // Convert UTC to Local Time (Sri Lanka Standard Time)
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");

            var posts = await query.Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                // Convert UTC to Local Time
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(p.CreatedAt, localTimeZone),
                Author = new UserResponseDto // Nested DTO for Author
                {
                    Id = p.Author.Id,
                    UserName = p.Author.UserName
                },
                AuthorRole = p.Author.Role.Name,
                Likes = p.Likes.Select(l => new LikeResponseDto // Nested DTO for Likes
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    PostId = l.PostId,
                    IsLike = l.IsLike
                }).ToList(),
                Comments = p.Comments.Select(c => new CommentResponseDto // Nested DTO for Comments
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(c.CreatedAt, localTimeZone),
                    UserName = c.Author.UserName,
                    UserRole = c.Author.Role.Name
                }).ToList(),
                LikeCount = p.Likes.Count(l => l.IsLike),
                DislikeCount = p.Likes.Count(l => !l.IsLike)
            }).ToListAsync();

            return Ok(posts);
        }

        [HttpGet("Trending")]
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


        [HttpPost]
        public async Task<ActionResult<PostResponseDto>> CreatePost([FromBody] PostRequestDto postRequestDto)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Create new Post Object
                var post = new Post
                {
                    Title = postRequestDto.Title,
                    Content = postRequestDto.Content,
                    AuthorId = postRequestDto.AuthorId,
                };

                // Save to database
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the post : " + ex.Message);
            }
        }

        
        [HttpPut]
        public async Task<ActionResult<PostResponseDto>> UpdatePost([FromBody] PostRequestDto updatePostRequestDto)
        {

            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Find the Existing Post by Post Id
                var existingPost = await _context.Posts.FindAsync(updatePostRequestDto.Id);

                if (existingPost == null)
                    return NotFound("Post not Found");

                // Swap the new values to Old Post
                existingPost.Title = updatePostRequestDto.Title;
                existingPost.Content = updatePostRequestDto.Content;

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPosts), new { id = existingPost.Id }, existingPost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updation the post : " + ex.Message);
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            // Find the Existing Post by Post Id
            var post = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) 
                return NotFound("Post Not Found");

            // There are two relationships with post
            // 1. Post -> Comment
            // 2. Post -> Like
            // When deleting a Post need to set Casecade or remove manually them

            // Remove manually
            _context.Comments.RemoveRange(post.Comments);
            _context.Likes.RemoveRange(post.Likes);

            // Remove the post
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

      
        [HttpPost("likes")]
        public async Task<IActionResult> AddLike([FromBody] LikeRequestDto likeRequestDto)
        {
            var post = await _context.Posts
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == likeRequestDto.PostId);

            if (post == null)
                return NotFound("Post Not Found");

            // Check if the user has already liked/disliked the post
            var existingLike = post.Likes
                .FirstOrDefault(l => l.UserId == likeRequestDto.UserId);

            if (existingLike != null)
            {
                // User has already liked/disliked the post, update the existing record
                existingLike.IsLike = likeRequestDto.isLike;
            }
            else
            {
                // User has not liked/disliked the post yet, create a new record
                var like = new Like
                {
                    PostId = likeRequestDto.PostId,
                    UserId = likeRequestDto.UserId,
                    IsLike = likeRequestDto.isLike
                };
                _context.Likes.Add(like);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
