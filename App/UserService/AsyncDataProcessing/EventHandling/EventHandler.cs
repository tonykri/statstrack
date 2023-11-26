using System.Text.Json;
using UserService.Dto.MessageBus.Received;
using UserService.Models;

namespace UserService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    public EventHandler(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public void BusinessCreated(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessCreatedRenewedDto>(message);
        if (eventDto is null) return;
        var user = dataContext.Users.First(u => u.Id == eventDto.UserId);
        user.NoOfBusinesses += 1;
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Created");
    }

    public void BusinessDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessUpdatedDeletedDto>(message);
        if (eventDto is null) return;
        var user = dataContext.Users.First(u => u.Id == eventDto.UserId);
        user.NoOfBusinesses -= 1;
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Deleted");
    }
}