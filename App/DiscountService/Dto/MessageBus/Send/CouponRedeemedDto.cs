namespace DiscountService.Dto.MessageBus.Send;

public class CouponRedeemedDto
{
    public CouponRedeemedDto(Guid userId, Guid businessId)
    {
        UserId = userId;
        BusinessId = businessId;
    }

    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public string Event {get; set; } 
}