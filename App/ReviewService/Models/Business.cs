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
    public string Brand { get; set; } = null!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<VerifiedOrder> VerifiedOrders { get; set; } = new List<VerifiedOrder>();
    public ICollection<Response> Responses { get; set; } = new List<Response>();
}