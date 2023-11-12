using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models;

public class Review
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid BusinessId { get; set; }
    [Required]
    public Business Business {get; set; }
    [Required]
    [Range(1,5)]
    public int Stars { get; set; }
    [Required]
    [MinLength(2)]
    public string Content { get; set; }
    [Required]
    public DateTime LastModified { get; set; } = DateTime.Now;
    public Response Response { get; set; }
}