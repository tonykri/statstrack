using System.ComponentModel.DataAnnotations;

namespace ReviewService.Dto;

public class CreateUpdateResponseDto
{
    public required Guid ReviewId { get; set; }
    public Guid BusinessId { get; set; }
    [MinLength(2)]
    public required string Content { get; set; }
}