using System.Text.Json;
using LocationService.Dto.MessageBus.Received;
using LocationService.Models;
namespace LocationService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    public EventHandler(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public void UserDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<UserDeletedDto>(message);
        if (eventDto is null) return;
        dataContext.Locations.RemoveRange(dataContext.Locations.Where(l => l.UserId == eventDto.UserId));
        dataContext.SaveChanges();
        Console.WriteLine("--> User's locations Deleted");
    }
}