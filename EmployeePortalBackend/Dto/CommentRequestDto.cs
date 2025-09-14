using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Dto
{
    public class CommentRequestDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
