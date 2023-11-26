namespace PaymentService.Dto.MessageBus.Send;

public class BusinessCreatedRenewedDto
{
    public BusinessCreatedRenewedDto(Guid businessId, Guid userId, DateTime expirationDate)
    {
        BusinessId = businessId;
        UserId = userId;
        ExpirationDate = expirationDate;
    }

    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
}