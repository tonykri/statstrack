using System.ComponentModel.DataAnnotations;

namespace UserService.Dto.Account;

public class RegisterCredentialsDto
{
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    [MinLength(3)]
    public required string FullName { get; set; }
}