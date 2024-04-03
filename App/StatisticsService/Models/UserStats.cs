using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class UserStats
{
    public UserStats(Business business, DateTime startTime, DateTime endTime)
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
    public int TotalUsers { get; set; } = 0;
    [Required]
    public int Males { get; set; } = 0;
    [Required]
    public int Females { get; set; } = 0;
    [Required]
    public int Youngers { get; set; } = 0;
    [Required]
    public int Adults { get; set; } = 0;
    [Required]
    public int OlderAdults { get; set; } = 0;
}