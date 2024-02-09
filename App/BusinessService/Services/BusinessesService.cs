using BusinessService.AsymcDataProcessing.MessageBusClient;
using BusinessService.Categories;
using BusinessService.Dto;
using BusinessService.Dto.MessageBus.Send;
using BusinessService.Models;
using BusinessService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Services;

public class BusinessesService : IBusinessesService {
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public BusinessesService(DataContext dataContext, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
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

    public async Task<ApiResponse<int, Exception>> UpdateBusiness(BusinessDto business)
    {
            Guid userId = tokenDecoder.GetUserId();
            var storedBusiness = await dataContext.Businesses.FirstOrDefaultAsync(b => b.Id == business.BID);

            if(storedBusiness is null)
                return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            if(storedBusiness.UserId != userId)
                return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));
            if(!CheckCategory(business.Category))
                return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_VALID));

            storedBusiness.Brand = business.Brand;
            storedBusiness.Description = business.Description;
            storedBusiness.Category = business.Category;
            storedBusiness.Address = business.Address;
            storedBusiness.Latitude = business.Latitude;
            storedBusiness.Longitude = business.Longitude;

            await dataContext.SaveChangesAsync();
            BusinessUpdatedDto businessDataDto = new BusinessUpdatedDto(storedBusiness.Id, storedBusiness.Brand);
            messageBusClient.UpdateBusiness(businessDataDto);
            
            return new ApiResponse<int, Exception>(0);
    }
}