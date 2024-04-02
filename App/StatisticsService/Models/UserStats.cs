using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class UserStats
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
    public int TotalUsers { get; set; }
    [Required]
    public int Males { get; set; }
    [Required]
    public int Females { get; set; }
    [Required]
    public int Youngers { get; set; }
    [Required]
    public int Adults { get; set; }
    [Required]
    public int OlderAdults { get; set; }
}