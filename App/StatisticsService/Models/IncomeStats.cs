using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class IncomeStats
{
    public IncomeStats(){}
    public IncomeStats(Business business, DateTime startTime, DateTime endTime)
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
    public int PovertyLine { get; set; } = 0;
    [Required]
    public int LowIncome { get; set; } = 0;
    [Required]
    public int LowerMiddleIncome { get; set; } = 0;
    [Required]
    public int MedianIncome { get; set; } = 0;
    [Required]
    public int UpperMiddleIncome { get; set; } = 0;
    [Required]
    public int HighIncome { get; set; } = 0;
    [Required]
    public int VeryHighIncome { get; set; } = 0;
}