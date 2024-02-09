using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class PersonalLife
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public bool StayHome { get; set; }

    [Required]
    public bool Married { get; set; }
}