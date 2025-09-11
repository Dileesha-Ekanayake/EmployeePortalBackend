using EmployeePortalBackend.Models;

namespace EmployeePortalBackend.Dto
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
