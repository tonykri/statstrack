using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class ExpenseStats
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
    public int HousingExpenses { get; set; }

    [Required]
    public int Utilities { get; set; }

    [Required]
    public int TransportationCosts { get; set; }

    [Required]
    public int FoodAndGroceries { get; set; }

    [Required]
    public int HealthcareExpenses { get; set; }

    [Required]
    public int EducationExpenses { get; set; }

    [Required]
    public int DebtPayments { get; set; }

    [Required]
    public int InsurancePremiums { get; set; }

    [Required]
    public int EntertainmentAndLeisure { get; set; }

    [Required]
    public int PersonalCareAndGrooming { get; set; }

    [Required]
    public int ClothingAndApparel { get; set; }

    [Required]
    public int SavingsAndInvestments { get; set; }

    [Required]
    public int Taxes { get; set; }

    [Required]
    public int ChildcareAndEducation { get; set; }

    [Required]
    public int HomeAndGarden { get; set; }

    [Required]
    public int SubscriptionsAndMemberships { get; set; }

    [Required]
    public int CharitableDonations { get; set; }

    [Required]
    public int TravelAndVacation { get; set; }

    [Required]
    public int LegalAndFinancialServices { get; set; }

    [Required]
    public int EmergencyFundAndContingencies { get; set; }

    [Required]
    public int Other { get; set; }
}
