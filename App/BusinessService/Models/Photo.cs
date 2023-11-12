using System.ComponentModel.DataAnnotations;

namespace BusinessService.Models;

public class Photo
{
    [Required]
    public Guid PhotoId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BusinessId { get; set; }
    public Business Business { get; set; }
    public string PhotoUri { get; set; }
}