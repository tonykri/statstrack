namespace UserService.Dto.Profile;

public class UserDto
{
    public DateOnly Birthdate { get; set; }
    public required string PhoneNumber { get; set; }
    public required string DialingCode { get; set; }
    public required string Gender { get; set; }
    public required string Country { get; set; }
}