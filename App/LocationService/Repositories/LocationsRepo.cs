using LocationService.Dto;
using LocationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Repositories;

public class LocationsRepo : ILocationsRepo
{
    private readonly DataContext dataContext;
    public LocationsRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<List<Guid>> GetUsersId(double businessLat, double businessLong, DateTime startTime, DateTime endTime, double radiusInMeters = 50)
    {

        double radiusInDegrees = radiusInMeters / 111000;

        return await dataContext.Locations
            .Where(l => Math.Abs(l.Latitude - businessLat) <= radiusInDegrees && Math.Abs(l.Longitude - businessLong) <= radiusInDegrees
                    && l.CreationDate >= startTime && l.CreationDate <= endTime)
            .Select(l => l.UserId)
            .Distinct()
            .ToListAsync();
    }
}