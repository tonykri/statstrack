namespace AccountService.Dto.MessageBus;

public class UserRegisteredDto
{
    public UserRegisteredDto(Guid userId, string firstName, string lastName, string email)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public Guid UserId { get; set; }
    public string Event {get; set; } = "User_Registered";
    public string FirstName { get; set; }
    public string LastName { get; set;}
    public string Email { get; set; }
    public string ProfileStage { get; set; } = "EmailConfirmation";
}