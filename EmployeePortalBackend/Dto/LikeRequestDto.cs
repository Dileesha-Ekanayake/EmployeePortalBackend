using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Dto
{
    public class LikeRequestDto
    {
        [Required]
        public bool isLike { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}
