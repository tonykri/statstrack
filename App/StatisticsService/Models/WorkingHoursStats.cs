using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class WorkingHoursStats
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
    public int FullTimeEmployment { get; set; }

    [Required]
    public int PartTimeEmployment { get; set; }

    [Required]
    public int OvertimeHours { get; set; }

    [Required]
    public int ShiftWork { get; set; }

    [Required]
    public int FlexibleHours { get; set; }

    [Required]
    public int SeasonalWork { get; set; }

    [Required]
    public int FreelancingWork { get; set; }

    [Required]
    public int ContractWork { get; set; }

    [Required]
    public int RemoteWork { get; set; }

    [Required]
    public int NightShifts { get; set; }

    [Required]
    public int WeekendWork { get; set; }

    [Required]
    public int Unemployed { get; set; }
}
