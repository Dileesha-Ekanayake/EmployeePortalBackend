namespace EmployeePortalBackend.Dto
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponseDto Author { get; set; }
        public string AuthorRole { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public List<CommentResponseDto> Comments { get; set; }
        
    }
}
