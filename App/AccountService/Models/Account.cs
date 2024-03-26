using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;

public class Account 
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Provider { get; set; } = "Credentials";
    [Required]
    public string ProfileStage { get; set; } = "EmailConfirmation";
    public ICollection<EmailCode> EmailCodes { get; set; } = new List<EmailCode>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}