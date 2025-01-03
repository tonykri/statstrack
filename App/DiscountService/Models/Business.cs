using System.ComponentModel.DataAnnotations;

namespace DiscountService.Models;

public class Business
{
    [Key]
    public Guid BusinessId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
    public string? Brand { get; set; }
    public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}