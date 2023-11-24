using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models;

public class Payment
{
    [Key]
    public string Id { get; set; }
    public Business Business { get; set; }
    public Guid BusinessId { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.Now;
}