namespace ReviewService.Dto.MessageBus.Received;

public class CouponRedeemedDto
{
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public string Event {get; set; }
}