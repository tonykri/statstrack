using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class HobbyStats
{
    public HobbyStats(Business business, DateTime startTime, DateTime endTime)
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
    public int Reading { get; set; } = 0;

    [Required]
    public int Gardening { get; set; } = 0;

    [Required]
    public int CookingAndBaking { get; set; } = 0;

    [Required]
    public int PaintingAndDrawing { get; set; } = 0;

    [Required]
    public int SportsAndPhysicalActivities { get; set; } = 0;

    [Required]
    public int Photography { get; set; } = 0;

    [Required]
    public int PlayingMusicalInstruments { get; set; } = 0;

    [Required]
    public int Traveling { get; set; } = 0;

    [Required]
    public int Crafting { get; set; } = 0;

    [Required]
    public int Fishing { get; set; } = 0;

    [Required]
    public int ModelBuilding { get; set; } = 0;

    [Required]
    public int Collecting { get; set; } = 0;

    [Required]
    public int HikingAndCamping { get; set; } = 0;

    [Required]
    public int DIYHomeImprovement { get; set; } = 0;

    [Required]
    public int BirdWatching { get; set; } = 0;

    [Required]
    public int Cycling { get; set; } = 0;

    [Required]
    public int MeditationAndMindfulness { get; set; } = 0;

    [Required]
    public int Dancing { get; set; } = 0;

    [Required]
    public int Volunteering { get; set; } = 0;

    [Required]
    public int BoardGamesAndPuzzles { get; set; } = 0;

    [Required]
    public int Writing { get; set; } = 0;

    [Required]
    public int FitnessAndExercise { get; set; } = 0;

    [Required]
    public int ComicsAndGraphicNovels { get; set; } = 0;

    [Required]
    public int Sculpting { get; set; } = 0;

    [Required]
    public int AstrologyAndAstronomy { get; set; } = 0;

    [Required]
    public int SewingAndQuilting { get; set; } = 0;

    [Required]
    public int Archery { get; set; } = 0;

    [Required]
    public int Genealogy { get; set; } = 0;

    [Required]
    public int MetalDetecting { get; set; } = 0;

    [Required]
    public int VintageAndClassicCars { get; set; } = 0;

    [Required]
    public int Other { get; set; } = 0;
}
