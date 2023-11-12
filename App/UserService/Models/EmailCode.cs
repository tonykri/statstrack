using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class EmailCode
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public DateTime CodeCreated { get; set; }
}