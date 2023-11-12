using System.ComponentModel.DataAnnotations;

namespace ReviewService.Dto;

public class CreateReviewDto
{
    public Guid BusinessId { get; set; }
    [Range(1,5)]
    public int Stars { get; set; }
    [MinLength(2)]
    public required string Content { get; set; }
}