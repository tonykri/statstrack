using System.Text.Json;
using AccountService.Dto.MessageBus.Received;
using AccountService.Models;

namespace AccountService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    public EventHandler(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public void UpdateProfileStage(string message)
    {
        var eventDto = JsonSerializer.Deserialize<UserProfileStageDto>(message);
        if (eventDto is null) return;

        var account = dataContext.Accounts.First(a => a.Id == eventDto.UserId);
        account.ProfileStage = eventDto.ProfileStage;
        dataContext.SaveChanges();
        
        Console.WriteLine("--> User's Profile Stage updated");
    }

}
