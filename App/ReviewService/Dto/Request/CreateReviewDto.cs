using System.ComponentModel.DataAnnotations;

namespace ReviewService.Dto;

public class CreateReviewDto
{
    public required Guid BusinessId { get; set; }
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
    public required int Stars { get; set; }
    [MinLength(1, ErrorMessage = "Content length must be at least 1 characters.")]
    [MaxLength(200, ErrorMessage = "Content length must be maximum 200 characters.")]
    public required string Content { get; set; }
}