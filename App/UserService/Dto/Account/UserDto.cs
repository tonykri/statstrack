using UserService.Models;

namespace UserService.Dto.Account;

public class UserDto
{
    public UserDto(User user)
    {
        Id = user.Id;
        FullName = user.FullName;
        Email = user.Email;
        BirthDate = new DateOnly(user.Birthdate.Year, user.Birthdate.Month, user.Birthdate.Day);
        PhoneNumber = user.PhoneNumber;
        Country = user.Country;
        PhotoUrl = user.PhotoUrl;
        Provider = user.Provider;
        NoOfBusinesses = user.NoOfBusinesses;
        ProfileStage = user.ProfileStage;
    }
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Provider { get; set; }
    public int NoOfBusinesses { get; set; }
    public string? ProfileStage { get; set; }
}