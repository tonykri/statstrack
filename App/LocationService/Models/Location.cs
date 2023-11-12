using System.ComponentModel.DataAnnotations;

namespace LocationService.Models;

public class Location
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}