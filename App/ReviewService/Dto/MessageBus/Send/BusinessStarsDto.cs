namespace ReviewService.Dto.MessageBus.Send;

public class BusinessStarsDto
{
    public Guid BusinessId { get; set; }
    public int Reviews { get; set; }
    public double Stars { get; set; }
    public string Event { get; set; } = "Reviews_Updated";
}