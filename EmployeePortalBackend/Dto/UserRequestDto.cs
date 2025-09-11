using EmployeePortalBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortalBackend.Dto
{
    public class UserRequestDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
