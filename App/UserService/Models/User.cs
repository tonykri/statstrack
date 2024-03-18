using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public string Provider { get; set; } = null!;
    public int NoOfBusinesses { get; set; } = 0;
    public string ProfileStage { get; set; }  = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
    public ProfessionalLife? UserProfessionalLife { get; set; }
    public PersonalLife? UserPersonalLife { get; set; }
    public EmailCode? UserEmailCode { get; set; }
}