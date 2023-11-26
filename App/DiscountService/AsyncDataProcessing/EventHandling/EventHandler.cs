using System.Text.Json;
using DiscountService.Dto.MessageBus.Received;
using DiscountService.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.AsymcDataProcessing.EventHandling;

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

    public void BusinessDeleted(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessUpdatedDeletedDto>(message);
        if (eventDto is null) return;

        var business = dataContext.Businesses.First(b => b.BusinessId == eventDto.BusinessId);
        dataContext.Remove(business);
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Deleted");
    }

    public void BusinessUpdated(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessUpdatedDeletedDto>(message);
        if (eventDto is null) return;

        var business = dataContext.Businesses.First(b => b.BusinessId == eventDto.BusinessId);
        business.Brand = eventDto.Body;
        dataContext.SaveChanges();
        Console.WriteLine("--> Business Updated");
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