namespace EmployeePortalBackend.Dto
{
    public class CommentRequestDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
