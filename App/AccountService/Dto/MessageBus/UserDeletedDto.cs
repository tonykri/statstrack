namespace AccountService.Dto.MessageBus;

public class UserDeletedDto
{
    public UserDeletedDto(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
    public string Event {get; set; } = "User_Deleted";
}