namespace AccountService.Dto.ExternalAuth;

public class GoogleTokenDto
{
    public string? access_token { get; set; }
    public string? id_token { get; set; }
}