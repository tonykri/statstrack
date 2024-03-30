using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;

public class EmailCode
{
    [Key]
    public Guid Id { get; set; } = new Guid();
    [Required]
    public Guid AccountId { get; set; }
    [Required]
    public Account Account { get; set; } = null!;
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public string Type { get; set; } = null!;
    [Required]
    public bool IsUsed { get; set; } = false;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(2);
}