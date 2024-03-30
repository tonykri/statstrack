using UserService.Categories;

namespace UserService.Dto.MessageBus.Send;

public class ProfileStageUpdatedDto 
{
    public ProfileStageUpdatedDto(Guid userId, ProfileStages name)
    {
        UserId = userId;
        ProfileStage = name.ToString();
    }

    public Guid UserId { get; set; }
    public string ProfileStage { get; set; }
    public string Event {get; set; } = "Profile_Stage_Updated";
}