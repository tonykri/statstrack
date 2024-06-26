using System.Text.Json;
using BusinessService.Dto.MessageBus.Received;
using BusinessService.Dto.MessageBus.Send;
using BusinessService.Models;
using BusinessService.Repositories;
using BusinessService.Services;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly DataContext dataContext;
    private readonly IBusinessPhotoService businessPhotoService;
    public EventHandler(DataContext dataContext, IBusinessPhotoService businessPhotoService)
    {
        this.dataContext = dataContext;
        this.businessPhotoService = businessPhotoService;
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

    public void ReviewsUpdated(string message)
    {
        var eventDto = JsonSerializer.Deserialize<BusinessStarsDto>(message);
        if (eventDto is null) return;
        var business = dataContext.Businesses.FirstOrDefault(b => b.Id == eventDto.BusinessId);
        if (business is null) return;
        business.Reviews = eventDto.Reviews;
        business.Stars = eventDto.Stars;
        dataContext.SaveChanges();
        Console.WriteLine("--> Reviews Updated");
    }

    public async void UserDeleted(string message)
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
                await businessPhotoService.DeletePhoto(photo.PhotoId);
        }
        dataContext.Businesses.RemoveRange(dataContext.Businesses.Where(b => b.UserId == eventDto.UserId));
        dataContext.SaveChanges();
        Console.WriteLine("--> Businesses Deleted");
    }
}