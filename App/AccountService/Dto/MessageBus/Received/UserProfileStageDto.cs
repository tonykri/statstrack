namespace AccountService.Dto.MessageBus.Received;

public class UserProfileStageDto
{
    public Guid UserId { get; set; }
    public string ProfileStage { get; set; }
}