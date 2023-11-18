using System.ComponentModel.DataAnnotations;

namespace ReviewService.Dto;

public class CreateUpdateResponseDto
{
    public required Guid ReviewId { get; set; }
    public required Guid BusinessId { get; set; }
    [MinLength(1, ErrorMessage = "Content length must be at least 1 characters.")]
    [MaxLength(200, ErrorMessage = "Content length must be maximum 200 characters.")]
    public required string Content { get; set; }
}