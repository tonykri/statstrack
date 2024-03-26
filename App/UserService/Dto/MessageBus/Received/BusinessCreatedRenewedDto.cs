namespace UserService.Dto.MessageBus.Received;

public class BusinessCreatedRenewedDto
{
    public Guid BusinessId { get; }
    public Guid UserId { get; }
    public DateTime ExpirationDate { get; }
}