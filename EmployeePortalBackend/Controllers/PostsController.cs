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

        // GET: api/posts (List all with author, timestamp, likes/dislikes)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPosts(string? author = null)
        {
            var query = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .AsQueryable(); // LINQ (Language Integrated Query)
                                // extension method that converts an object that implements IEnumerable<T> into an IQueryable<T>.

            // Filter by User (Author) name
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(p => p.Author.UserName.Contains(author));
            }

            var posts = await query.Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                AuthorName = p.Author.UserName,
                LikeCount = p.Likes.Count(l => l.IsLike),
                DislikeCount = p.Likes.Count(l => !l.IsLike)
            }).ToListAsync();

            return Ok(posts);
        }

        // POST: api/posts (Create post)
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

        // PUT: api/posts/{id} (Update own post)
        [HttpPut]
        public async Task<ActionResult<PostResponseDto>> UpdatePost(int id, [FromBody] PostRequestDto updatePostRequestDto)
        {

            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Find the Existing Post by Post Id
                var existingPost = await _context.Posts.FindAsync(id);

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

        // DELETE: api/posts/{id} (Delete own post)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            // Find the Existing Post by Post Id
            var post = await _context.Posts.FindAsync(id);

            if (post == null) 
                return NotFound("Post Not Found");

            // Remove the post
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/posts/{postId}/like (Like/Dislike)
        [HttpPost("{postId}/like/{userId}")]
        public async Task<IActionResult> AddLike(int postId, int userId, [FromBody] bool isLike)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
                return NotFound("Post Not Found");

            var like = new Like
            {
                PostId = postId,
                IsLike = isLike,
                UserId = userId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
