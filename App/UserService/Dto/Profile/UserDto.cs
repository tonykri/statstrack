using System.ComponentModel.DataAnnotations;

namespace UserService.Dto.Profile;

public class UserDto
{
    [Required]
    public DateOnly Birthdate { get; set; }
    [MinLength(10)]
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }
    [MinLength(1)]
    [MaxLength(5)]
    public required string DialingCode { get; set; }
    public required string Gender { get; set; }
    public required string Country { get; set; }
}