using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class ExpenseStats
{
    public ExpenseStats(Business business, DateTime startTime, DateTime endTime)
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
    public int HousingExpenses { get; set; } = 0;

    [Required]
    public int Utilities { get; set; } = 0;

    [Required]
    public int TransportationCosts { get; set; } = 0;

    [Required]
    public int FoodAndGroceries { get; set; } = 0;

    [Required]
    public int HealthcareExpenses { get; set; } = 0;

    [Required]
    public int EducationExpenses { get; set; } = 0;

    [Required]
    public int DebtPayments { get; set; } = 0;

    [Required]
    public int InsurancePremiums { get; set; } = 0;

    [Required]
    public int EntertainmentAndLeisure { get; set; } = 0;

    [Required]
    public int PersonalCareAndGrooming { get; set; } = 0;

    [Required]
    public int ClothingAndApparel { get; set; } = 0;

    [Required]
    public int SavingsAndInvestments { get; set; } = 0;

    [Required]
    public int Taxes { get; set; } = 0;

    [Required]
    public int ChildcareAndEducation { get; set; } = 0;

    [Required]
    public int HomeAndGarden { get; set; } = 0;

    [Required]
    public int SubscriptionsAndMemberships { get; set; } = 0;

    [Required]
    public int CharitableDonations { get; set; } = 0;

    [Required]
    public int TravelAndVacation { get; set; } = 0;

    [Required]
    public int LegalAndFinancialServices { get; set; } = 0;

    [Required]
    public int EmergencyFundAndContingencies { get; set; } = 0;

    [Required]
    public int Other { get; set; } = 0;
}
