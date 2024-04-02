using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class Business
{
    [Key]
    public Guid BusinessId { get; set; } 
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
    public ICollection<EducationStats> EducationStats { get; set; } = new List<EducationStats>();
    public ICollection<HobbyStats> HobbyStats { get; set; } = new List<HobbyStats>();
    public ICollection<ExpenseStats> ExpenseStats { get; set; } = new List<ExpenseStats>();
    public ICollection<IncomeStats> IncomeStats { get; set; } = new List<IncomeStats>();
    public ICollection<IndustryStats> IndustryStats { get; set; } = new List<IndustryStats>();
    public ICollection<PersonalStats> PersonalStats { get; set; } = new List<PersonalStats>();
    public ICollection<UserStats> UserStats { get; set; } = new List<UserStats>();
    public ICollection<WorkingHoursStats> WorkingHoursStats { get; set; } = new List<WorkingHoursStats>();
}