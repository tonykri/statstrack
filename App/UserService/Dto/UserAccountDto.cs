namespace UserService.Dto;

public class UserAccountDto
{
    public DateTime Birthdate { get; set; }
    public string Gender { get; set; } = null!;
    public ICollection<string> Expenses { get; set; } = new List<string>();
    public ICollection<string> Hobbies { get; set; } = new List<string>();
    public string LevelOfEducation { get; set; } = null!;
    public string Industry { get; set; } = null!;
    public string Income { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public bool StayHome { get; set; }
    public bool Married { get; set; }
}
