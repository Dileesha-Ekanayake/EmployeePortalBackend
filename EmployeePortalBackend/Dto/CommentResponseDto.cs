namespace EmployeePortalBackend.Dto
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
