namespace BusinessService.Dto.MessageBus.Send;

public class BusinessUpdatedDto
{
    public BusinessUpdatedDto(Guid businessId, string brand)
    {
        BusinessId = businessId;
        Body = brand;
    }

    public Guid BusinessId { get; set; }
    public Guid? UserId { get; set; }
    public string? Body { get; set; }
    public string Event {get; set; } = "Business_Updated";
}