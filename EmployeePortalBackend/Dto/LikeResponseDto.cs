namespace EmployeePortalBackend.Dto
{
    public class LikeResponseDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool IsLike { get; set; }
    }
}
