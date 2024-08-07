using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class EducationStats
{
    public EducationStats() { }
    public EducationStats(Business business, DateTime startTime, DateTime endTime)
    {
        BusinessId = business.BusinessId;
        Business = business;
        StartTime = startTime;
        EndTime = endTime;
    }
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
    public int Primary { get; set; } = 0;

    [Required]
    public int Secondary { get; set; } = 0;

    [Required]
    public int Higher { get; set; } = 0;

    [Required]
    public int Technical { get; set; } = 0;
}
