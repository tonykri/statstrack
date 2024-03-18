using System.ComponentModel.DataAnnotations;

namespace UserService.Dto.Account;

public class RegisterCredentialsDto
{
    [EmailAddress]
    [MaxLength(100, ErrorMessage = "Email cannot be over 100 characters")]
    public required string Email { get; set; }
    [MinLength(3 , ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage = "Name must be maximum 100 characters")]
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}