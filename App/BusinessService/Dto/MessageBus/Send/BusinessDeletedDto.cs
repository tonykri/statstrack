namespace BusinessService.Dto.MessageBus.Send;

public class BusinessDeletedDto
{
    public BusinessDeletedDto(Guid businessId, Guid userId)
    {
        BusinessId = businessId;
        UserId = userId;
    }

    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public string Event {get; set; } = "Business_Deleted";
}