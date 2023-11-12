using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models;

public class Response
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? ReviewId { get; set; }
    [Required]
    public Review Review { get; set; }
    [Required]
    public Guid BusinessId { get; set; }
    [Required]
    public Business Business { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public DateTime LastModified { get; set; } = DateTime.Now;
}