using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Email { get; set; }
    [Required]
    public string FullName { get; set; }
    public DateTime Birthdate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public string Provider { get; set; }
    public int NoOfBusinesses { get; set; } = 0;
    public string ProfileStage { get; set; } 
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<Hobby>? Hobbies { get; set; }
    public ProfessionalLife? UserProfessionalLife { get; set; }
    public PersonalLife? UserPersonalLife { get; set; }
    public EmailCode? UserEmailCode { get; set; }
}