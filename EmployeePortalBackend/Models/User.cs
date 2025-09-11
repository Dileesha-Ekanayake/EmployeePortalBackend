using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortalBackend.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [ForeignKey("Role")]
        [Column("role_id")]
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
