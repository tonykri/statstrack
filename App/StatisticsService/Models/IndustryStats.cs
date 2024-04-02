using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class IndustryStats
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
    public int InformationTechnology { get; set; }

    [Required]
    public int Healthcare { get; set; }

    [Required]
    public int FinanceAndBanking { get; set; }

    [Required]
    public int Education { get; set; }

    [Required]
    public int Engineering { get; set; }

    [Required]
    public int ManufacturingAndProduction { get; set; }

    [Required]
    public int Retail { get; set; }

    [Required]
    public int HospitalityAndTourism { get; set; }

    [Required]
    public int MarketingAndAdvertising { get; set; }

    [Required]
    public int GovernmentAndPublicAdministration { get; set; }

    [Required]
    public int Legal { get; set; }

    [Required]
    public int Agriculture { get; set; }

    [Required]
    public int MediaAndJournalism { get; set; }

    [Required]
    public int ArtAndDesign { get; set; }

    [Required]
    public int NonprofitAndSocialServices { get; set; }

    [Required]
    public int EnvironmentalAndSustainability { get; set; }

    [Required]
    public int ConstructionAndSkilledTrades { get; set; }

    [Required]
    public int Automotive { get; set; }

    [Required]
    public int TransportationAndLogistics { get; set; }

    [Required]
    public int SportsAndFitness { get; set; }

    [Required]
    public int OnlineBusinesses { get; set; }

    [Required]
    public int PharmaceuticalsAndHealthcareResearch { get; set; }

    [Required]
    public int EntertainmentAndArts { get; set; }

    [Required]
    public int TechStartupsAndEntrepreneurship { get; set; }

    [Required]
    public int RealEstateAndPropertyManagement { get; set; }

    [Required]
    public int FoodAndBeverage { get; set; }

    [Required]
    public int AviationAndAerospace { get; set; }

    [Required]
    public int RenewableEnergyAndGreenTechnology { get; set; }

    [Required]
    public int FitnessAndWellness { get; set; }

    [Required]
    public int EmergencyServicesAndPublicSafety { get; set; }

    [Required]
    public int Student { get; set; }

    [Required]
    public int Retired { get; set; }
}