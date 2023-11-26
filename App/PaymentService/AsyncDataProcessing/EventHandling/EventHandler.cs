using System.Text.Json;
using PaymentService.Dto.MessageBus.Received;
using PaymentService.Models;

namespace PaymentService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    public EventHandler(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public void BusinessDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessUpdatedDeletedDto>(message);
        if (eventDto is null) return;

        var business = dataContext.Businesses.First(b => b.BusinessId == eventDto.BusinessId);
        dataContext.Remove(business);
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Deleted");
    }

    public void UserDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<UserDeletedDto>(message);
        if (eventDto is null) return;
        dataContext.Businesses.RemoveRange(dataContext.Businesses.Where(b => b.UserId == eventDto.UserId));
        dataContext.SaveChanges();
        Console.WriteLine("--> Businesses Deleted");
    }
}