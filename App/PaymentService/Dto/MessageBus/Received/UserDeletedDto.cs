namespace PaymentService.Dto.MessageBus.Received;

public class UserDeletedDto
{
    public Guid UserId { get; set; }
    public string Event {get; set; } = null!;
}