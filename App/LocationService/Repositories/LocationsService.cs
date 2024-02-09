using LocationService.Dto;
using LocationService.Models;
using LocationService.Utils;

namespace LocationService.Services;

public class LocationsService : ILocationsService
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public LocationsService(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public async Task<ApiResponse<int, Exception>> PostLocation(LocationDto location)
    {
        Guid userId = tokenDecoder.GetUserId();
        await dataContext.AddAsync(new Location
        {
            UserId = userId,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        });
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }
}