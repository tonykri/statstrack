using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class ProfessionalLife
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    public string LevelOfEducation { get; set; }

    [Required]
    public string Industry { get; set; }

    [Required]
    public string Income { get; set; }

    [Required]
    public string WorkingHours { get; set; }

}