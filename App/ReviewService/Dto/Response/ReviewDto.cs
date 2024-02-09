namespace ReviewService.Dto;

public class ReviewDto
{
    public class ReviewResponse
    {
        public string? Content { get; set; }
        public DateTime LastModified { get; set; }
    }
    public Guid Id { get; set; }
    public int Stars { get; set; }
    public string? Content { get; set; }
    public DateTime LastModified { get; set; }
    public ReviewResponse? Response { get; set; }
}