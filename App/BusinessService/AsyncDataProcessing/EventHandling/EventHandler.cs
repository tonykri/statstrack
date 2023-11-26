using System.Text.Json;
using BusinessService.Dto.MessageBus.Received;
using BusinessService.Models;
using BusinessService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    private readonly IBusinessPhotosRepo businessPhotosRepo;
    public EventHandler(DataContext dataContext, IBusinessPhotosRepo businessPhotosRepo)
    {
        this.dataContext = dataContext;
        this.businessPhotosRepo = businessPhotosRepo;
    }
    
    public void BusinessCreated(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessCreatedRenewedDto>(message);
        if (eventDto is null) return;
        Business business = new Business()
        {
            Id = eventDto.BusinessId,
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
        List<ICollection<Photo>> urls = dataContext.Businesses
            .Include(b => b.Photos)
            .Where(b => b.UserId == eventDto.UserId)
            .Select(b => b.Photos)
            .ToList();
        foreach(ICollection<Photo> burls in urls)
        {
            foreach(Photo photo in burls)
                businessPhotosRepo.DeletePhoto(photo.PhotoId);
        }
        dataContext.Businesses.RemoveRange(dataContext.Businesses.Where(b => b.UserId == eventDto.UserId));
        dataContext.SaveChanges();
        Console.WriteLine("--> Businesses Deleted");
    }
}