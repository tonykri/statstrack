namespace UserService.Dto.MessageBus.Received;

public class AccountUpdatedDto
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileStage { get; set; }
}