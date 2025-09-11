using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Dto
{
    public class PostRequestDto
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
