using System.ComponentModel.DataAnnotations;

namespace BusinessService.Dto;

public class BusinessDto
{
    public required Guid BID { get; set; }
    [MinLength(1), MaxLength(50)]
    public required string Brand { get; set; }
    [MinLength(5), MaxLength(200)]
    public required string Description { get; set; }
    public required string Category { get; set; }
    [MinLength(5), MaxLength(100)]
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}