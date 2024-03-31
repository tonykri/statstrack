namespace UserService.Dto.MessageBus.Received;

public class BusinessUpdatedDeletedDto
{
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
}