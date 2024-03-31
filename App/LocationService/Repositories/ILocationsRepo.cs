using LocationService.Dto;

namespace LocationService.Repositories;

public interface ILocationsRepo
{
    Task<List<Guid>> GetUsersId(double businessLat, double businessLong, DateTime startTime, DateTime endTime, double radiusInMeters = 50);
}