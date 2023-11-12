using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models;

public class Business
{
    [Key]
    public Guid BusinessId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
    [Required]
    public string Brand { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<VerifiedOrder>? VerifiedOrders { get; set; }
    public ICollection<Response>? Responses { get; set; }
}