using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class WorkingHoursStats
{
    public WorkingHoursStats(){}
    public WorkingHoursStats(Business business, DateTime startTime, DateTime endTime)
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
    public int FullTimeEmployment { get; set; } = 0;

    [Required]
    public int PartTimeEmployment { get; set; } = 0;

    [Required]
    public int OvertimeHours { get; set; } = 0;

    [Required]
    public int ShiftWork { get; set; } = 0;

    [Required]
    public int FlexibleHours { get; set; } = 0;

    [Required]
    public int SeasonalWork { get; set; } = 0;

    [Required]
    public int FreelancingWork { get; set; } = 0;

    [Required]
    public int ContractWork { get; set; } = 0;

    [Required]
    public int RemoteWork { get; set; } = 0;

    [Required]
    public int NightShifts { get; set; } = 0;

    [Required]
    public int WeekendWork { get; set; } = 0;

    [Required]
    public int Unemployed { get; set; } = 0;
}
