using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories;

public class BusinessRepo : IBusinessRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public BusinessRepo(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public async Task<ApiResponse<Business, Exception>> GetBusiness(Guid businessId)
    {
        var storedBusiness = await dataContext.Businesses
            .Include(b => b.Photos)
            .FirstOrDefaultAsync(b => b.Id == businessId && b.ExpirationDate > DateTime.UtcNow);
        if (storedBusiness is null)
            return new ApiResponse<Business, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        return new ApiResponse<Business, Exception>(storedBusiness);
    }

    public async Task<ApiResponse<List<Business>, Exception>> GetMyBusinesses()
    {
        Guid userId = tokenDecoder.GetUserId();
        var storedBusinesses = await dataContext.Businesses
            .Include(b => b.Photos)
            .Where(b => b.UserId == userId).ToListAsync();
        return new ApiResponse<List<Business>, Exception>(storedBusinesses);
    }

    public async Task<ApiResponse<List<Business>, Exception>> GetBusinesses(double upperLat, double upperLong, double bottomLat, double bottomLong, string? category = null)
    {
        var businesses = new List<Business>();
        if (category is null)
            businesses = await dataContext.Businesses
                .Include(b => b.Photos)
                .Where(b =>
                    b.Latitude <= upperLat &&
                    b.Latitude >= bottomLat &&
                    b.Longitude <= upperLong &&
                    b.Longitude >= bottomLong &&
                    b.ExpirationDate > DateTime.UtcNow)
                .ToListAsync();
        else
            businesses = await dataContext.Businesses
                .Include(b => b.Photos)
                .Where(b =>
                    b.Latitude <= upperLat &&
                    b.Latitude >= bottomLat &&
                    b.Longitude <= upperLong &&
                    b.Longitude >= bottomLong &&
                    b.ExpirationDate > DateTime.UtcNow &&
                    b.Category != null &&
                    b.Category.Equals(category))
                .ToListAsync();

        return new ApiResponse<List<Business>, Exception>(businesses);
    }
}