using System.Text.Json;
using StatisticsService.Dto.MessageBus;
using StatisticsService.Models;
using Microsoft.EntityFrameworkCore;
using StatisticsService.Dto.MessageBus.Received;

namespace StatisticsService.AsymcDataProcessing.EventHandling;

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
        Business business = new Business()
        {
            BusinessId = eventDto.BusinessId,
            UserId = eventDto.UserId,
            ExpirationDate = eventDto.ExpirationDate
        };
        dataContext.Add(business);
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Created");
    }

    public void UserDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<UserDeletedDto>(message);
        if (eventDto is null) return;

        dataContext.Businesses.RemoveRange(dataContext.Businesses.Where(b => b.UserId == eventDto.UserId));
        dataContext.SaveChanges();
        Console.WriteLine("--> Businesses and Stats Deleted");
    }
}