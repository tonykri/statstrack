using System.ComponentModel.DataAnnotations;

namespace BusinessService.Models;

public class Business
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime ExpirationDate { get; set; }
    public double Stars { get; set; } = 0;
    public ICollection<Photo> Photos { get; set; }
}