using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Auth
{
    public class LogInRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
