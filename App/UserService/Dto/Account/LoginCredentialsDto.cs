namespace UserService.Dto.Account;

public class LoginCredentialsDto
{
    [EmailAddress]
    [MaxLength(100, ErrorMessage = "Email cannot be over 100 characters")]
    public required string Email { get; set; }
    public required string Code { get; set; }
}