using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;

public class RefreshToken 
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid AccountId { get; set; }
    [Required]
    public Account Account { get; set; } = null!;
    [Required]
    public string Token { get; set; } = null!;
    [Required]
    public DateTime FirstLoginDate { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime RefreshDate { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime ExpiringDate { get; set; } = DateTime.UtcNow.AddDays(7);
}