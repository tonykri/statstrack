using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models;

public class Business
{
    [Key]
    public Guid BusinessId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
    [Required]
    public string? Brand { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}