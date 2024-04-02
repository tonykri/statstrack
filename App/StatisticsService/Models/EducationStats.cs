using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class EducationStats
{
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BusinessId { get; set; }
    [Required]
    public Business Business { get; set; } = null!;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public int Primary { get; set; }

    [Required]
    public int Secondary { get; set; }

    [Required]
    public int Higher { get; set; }

    [Required]
    public int Technical { get; set; }
}
