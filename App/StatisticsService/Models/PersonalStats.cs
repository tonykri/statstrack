using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class PersonalStats
{
    public PersonalStats(){}
    public PersonalStats(Business business, DateTime startTime, DateTime endTime)
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
    public int StayHome { get; set; } = 0;
    [Required]
    public int GoOut { get; set; } = 0;
    [Required]
    public int Married { get; set; } = 0;
    [Required]
    public int Single { get; set; } = 0;
}