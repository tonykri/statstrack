namespace PaymentService.Dto.MessageBus.Received;

public class BusinessUpdatedDeletedDto
{
    public Guid BusinessId { get; set; }
    public Guid? UserId { get; set; }
    public string? Body { get; set; }
    public string Event {get; set; } 
}