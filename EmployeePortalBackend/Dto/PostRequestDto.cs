using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Dto
{
    public class PostRequestDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
