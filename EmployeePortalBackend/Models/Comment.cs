using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortalBackend.Models
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [Column("content")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Post")]
        [Column("post_id")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
