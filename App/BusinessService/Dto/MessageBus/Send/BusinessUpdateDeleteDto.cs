namespace BusinessService.Dto.MessageBus.Send;

public class BusinessUpdatedDeletedDto
{
    public BusinessUpdatedDeletedDto(Guid businessId, Guid userId)
    {
        BusinessId = businessId;
        UserId = userId;
        Event = "Business_Deleted";
    }

    public BusinessUpdatedDeletedDto(Guid businessId, string brand)
    {
        BusinessId = businessId;
        Event = "Business_Updated";
        Body = brand;
    }

    public Guid BusinessId { get; set; }
    public Guid? UserId { get; set; }
    public string? Body { get; set; }
    public string Event {get; set; } 
}