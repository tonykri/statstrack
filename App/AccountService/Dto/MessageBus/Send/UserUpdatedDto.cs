using AccountService.Categories;

namespace AccountService.Dto.MessageBus.Send;

public class UserUpdatedDto
{
    public UserUpdatedDto(Guid userId, ProfileStages profileStage)
    {
        UserId = userId;
        ProfileStage = profileStage.ToString();
    }
    public UserUpdatedDto(Guid userId, string firstName, string lastName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileStage { get; set; }
    public string Event { get; } = "User_Data_Updated";
}