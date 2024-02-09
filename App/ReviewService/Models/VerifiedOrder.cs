using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models;

public class VerifiedOrder
{
    [Required]
    public Guid BusinessId { get; set; }
    [Required]
    public Business Business { get; set; } = null!;
    [Required]
    public Guid UserId { get; set; }
}