namespace UserService.Dto.MessageBus.Received;

public class UserRegisteredDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ProfileStage { get; set; } = null!;
}