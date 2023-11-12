using System.ComponentModel.DataAnnotations;

namespace DiscountService.Models;

public class Coupon
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BusinessId { get; set; } 
    [Required]
    public Business Business { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    public DateTime? RedeemDate { get; set; } = null;
}