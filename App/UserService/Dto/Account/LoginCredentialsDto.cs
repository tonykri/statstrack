namespace UserService.Dto.Account;

public class LoginCredentialsDto
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}