namespace BusinessService.Dto;

public class BusinessDto
{
    public required Guid BID { get; set; }
    public required string Brand { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}