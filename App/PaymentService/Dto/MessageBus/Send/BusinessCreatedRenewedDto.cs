namespace PaymentService.Dto.MessageBus.Send;

public class BusinessCreatedRenewedDto
{
    public BusinessCreatedRenewedDto(Guid businessId, Guid userId, DateTime expirationDate, string givenEvent)
    {
        BusinessId = businessId;
        UserId = userId;
        ExpirationDate = expirationDate;
        Event = givenEvent;
    }

    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Event { get; set; } 
}