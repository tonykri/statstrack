namespace UserService.Dto.MessageBus.Send;

public class UserUpdatedDto
{
    public UserUpdatedDto(Guid userId, string name)
    {
        UserId = userId;
        Body = name;
    }

    public Guid UserId { get; set; }
    public string Body { get; set; }
    public string Event {get; set; } = "User_Updated";
}