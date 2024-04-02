using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Models;

public class HobbyStats
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
    public int Reading { get; set; }

    [Required]
    public int Gardening { get; set; }

    [Required]
    public int CookingAndBaking { get; set; }

    [Required]
    public int PaintingAndDrawing { get; set; }

    [Required]
    public int SportsAndPhysicalActivities { get; set; }

    [Required]
    public int Photography { get; set; }

    [Required]
    public int PlayingMusicalInstruments { get; set; }

    [Required]
    public int Traveling { get; set; }

    [Required]
    public int Crafting { get; set; }

    [Required]
    public int Fishing { get; set; }

    [Required]
    public int ModelBuilding { get; set; }

    [Required]
    public int Collecting { get; set; }

    [Required]
    public int HikingAndCamping { get; set; }

    [Required]
    public int DIYHomeImprovement { get; set; }

    [Required]
    public int BirdWatching { get; set; }

    [Required]
    public int Cycling { get; set; }

    [Required]
    public int MeditationAndMindfulness { get; set; }

    [Required]
    public int Dancing { get; set; }

    [Required]
    public int Volunteering { get; set; }

    [Required]
    public int BoardGamesAndPuzzles { get; set; }

    [Required]
    public int Writing { get; set; }

    [Required]
    public int FitnessAndExercise { get; set; }

    [Required]
    public int ComicsAndGraphicNovels { get; set; }

    [Required]
    public int Sculpting { get; set; }

    [Required]
    public int AstrologyAndAstronomy { get; set; }

    [Required]
    public int SewingAndQuilting { get; set; }

    [Required]
    public int Archery { get; set; }

    [Required]
    public int Genealogy { get; set; }

    [Required]
    public int MetalDetecting { get; set; }

    [Required]
    public int VintageAndClassicCars { get; set; }

    [Required]
    public int Other { get; set; }
}
