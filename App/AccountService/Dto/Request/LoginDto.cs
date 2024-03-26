namespace AccountService.Dto.Request;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}
