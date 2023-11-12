namespace DiscountService.Dto;

public class CouponDto
{
    public Guid? BusinessId { get; set; }
    public string? Brand { get; set; }
    public string Code { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? RedeemDate { get; set; }
}