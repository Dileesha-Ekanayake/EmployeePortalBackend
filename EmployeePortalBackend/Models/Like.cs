using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortalBackend.Models
{
    [Table("like")]
    public class Like
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("is_like")]
        public bool IsLike { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Pots")]
        [Column("post_id")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
