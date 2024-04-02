using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class PersonalStats
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
    public int StayHome { get; set; }
    [Required]
    public int Married { get; set; }
}