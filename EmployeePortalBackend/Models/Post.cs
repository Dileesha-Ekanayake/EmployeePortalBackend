using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortalBackend.Models
{
    [Table("post")]
    public class Post
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Column("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [Column("content")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        [Column("user_id")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        // Computed properties for likes/dislikes
        [NotMapped]
        public int LikeCount => Likes?.Count(l => l.IsLike) ?? 0;
        [NotMapped]
        public int DislikeCount => Likes?.Count(l => !l.IsLike) ?? 0;
    }
}
