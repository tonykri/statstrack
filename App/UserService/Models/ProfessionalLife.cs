using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class ProfessionalLife
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public string LevelOfEducation { get; set; } = null!;

    [Required]
    public string Industry { get; set; } = null!;

    [Required]
    public string Income { get; set; } = null!;

    [Required]
    public string WorkingHours { get; set; } = null!;

}