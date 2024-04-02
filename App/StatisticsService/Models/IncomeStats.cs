using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class IncomeStats
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
    public int PovertyLine { get; set; }
    [Required]
    public int LowIncome { get; set; }
    [Required]
    public int LowerMiddleIncome { get; set; }
    [Required]
    public int MedianIncome { get; set; }
    [Required]
    public int UpperMiddleIncome { get; set; }
    [Required]
    public int HighIncome { get; set; }
    [Required]
    public int VeryHighIncome { get; set; }
}