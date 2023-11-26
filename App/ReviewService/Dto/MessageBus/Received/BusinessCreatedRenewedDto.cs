namespace ReviewService.Dto.MessageBus.Received;

public class BusinessCreatedRenewedDto
{
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
}