using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class EmailCode
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public DateTime CodeCreated { get; set; }
}