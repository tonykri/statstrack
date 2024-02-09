using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models;

public class Payment
{
    [Key]
    public string Id { get; set; } = null!;
    public Business Business { get; set; } = null!;
    public Guid BusinessId { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.Now;
}