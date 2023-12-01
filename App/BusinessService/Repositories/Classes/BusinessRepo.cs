using BusinessService.AsymcDataProcessing.MessageBusClient;
using BusinessService.Categories;
using BusinessService.Dto;
using BusinessService.Dto.MessageBus.Send;
using BusinessService.Models;
using BusinessService.Utils;

namespace BusinessService.Repositories;

public class BusinessRepo : IBusinessRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public BusinessRepo(DataContext dataContext, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }

    private bool CheckCategory(string category)
    {
        bool exists = false;
        foreach (Businesses businessCategory in Enum.GetValues(typeof(Businesses)))
        {
            if (businessCategory.ToString().Equals(category))
            {
                exists = true;
                break;
            }   
        }
        return exists;
    }

    public void UpdateBusiness(BusinessDto business)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var storedBusiness = dataContext.Businesses.FirstOrDefault(b => b.Id == business.BID);

            if(storedBusiness is null)
                throw new NotFoundException("Business not found");
            if(storedBusiness.UserId != userId)
                throw new NotAllowedException("Business does not belong to current user");
            if(!CheckCategory(business.Category))
                throw new NotValidException("Business category not valid");

            storedBusiness.Brand = business.Brand;
            storedBusiness.Description = business.Description;
            storedBusiness.Category = business.Category;
            storedBusiness.Address = business.Address;
            storedBusiness.Latitude = business.Latitude;
            storedBusiness.Longitude = business.Longitude;

            dataContext.SaveChanges();
            BusinessUpdatedDto businessDataDto = new BusinessUpdatedDto(storedBusiness.Id, storedBusiness.Brand);
            messageBusClient.UpdateBusiness(businessDataDto);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public Business GetBusiness(Guid businessId)
    {
        try
        {
            var storedBusiness = dataContext.Businesses.FirstOrDefault(b => b.Id == businessId && b.ExpirationDate < DateTime.Now);
            if(storedBusiness is null)
                throw new NotFoundException("Business not found");
            return storedBusiness;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public List<Business> GetMyBusinesses()
    {
        Guid userId = tokenDecoder.GetUserId();
        var storedBusinesses = dataContext.Businesses.Where(b => b.UserId == userId).ToList();
        return storedBusinesses;
    }

    public List<Business> GetBusinesses(double upperLat, double upperLong, double bottomLat, double bottomLong)
    {
        var storedBusinesses = dataContext.Businesses.Where(b => b.Latitude < upperLat && b.Latitude > bottomLat 
            && b.Longitude < upperLong && b.Longitude > bottomLong && b.ExpirationDate < DateTime.Now)
            .ToList();
        return storedBusinesses;
    }
}