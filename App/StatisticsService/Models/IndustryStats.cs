using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class IndustryStats
{
    public IndustryStats() {}
    public IndustryStats(Business business, DateTime startTime, DateTime endTime)
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
    public int InformationTechnology { get; set; } = 0;

    [Required]
    public int Healthcare { get; set; } = 0;

    [Required]
    public int FinanceAndBanking { get; set; } = 0;

    [Required]
    public int Education { get; set; } = 0;

    [Required]
    public int Engineering { get; set; } = 0;

    [Required]
    public int ManufacturingAndProduction { get; set; } = 0;

    [Required]
    public int Retail { get; set; } = 0;

    [Required]
    public int HospitalityAndTourism { get; set; } = 0;

    [Required]
    public int MarketingAndAdvertising { get; set; } = 0;

    [Required]
    public int GovernmentAndPublicAdministration { get; set; } = 0;

    [Required]
    public int Legal { get; set; } = 0;

    [Required]
    public int Agriculture { get; set; } = 0;

    [Required]
    public int MediaAndJournalism { get; set; } = 0;

    [Required]
    public int ArtAndDesign { get; set; } = 0;

    [Required]
    public int NonprofitAndSocialServices { get; set; } = 0;

    [Required]
    public int EnvironmentalAndSustainability { get; set; } = 0;

    [Required]
    public int ConstructionAndSkilledTrades { get; set; } = 0;

    [Required]
    public int Automotive { get; set; } = 0;

    [Required]
    public int TransportationAndLogistics { get; set; } = 0;

    [Required]
    public int SportsAndFitness { get; set; } = 0;

    [Required]
    public int OnlineBusinesses { get; set; } = 0;

    [Required]
    public int PharmaceuticalsAndHealthcareResearch { get; set; } = 0;

    [Required]
    public int EntertainmentAndArts { get; set; } = 0;

    [Required]
    public int TechStartupsAndEntrepreneurship { get; set; } = 0;

    [Required]
    public int RealEstateAndPropertyManagement { get; set; } = 0;

    [Required]
    public int FoodAndBeverage { get; set; } = 0;

    [Required]
    public int AviationAndAerospace { get; set; } = 0;

    [Required]
    public int RenewableEnergyAndGreenTechnology { get; set; } = 0;

    [Required]
    public int FitnessAndWellness { get; set; } = 0;

    [Required]
    public int EmergencyServicesAndPublicSafety { get; set; } = 0;

    [Required]
    public int Student { get; set; } = 0;

    [Required]
    public int Retired { get; set; } = 0;
}