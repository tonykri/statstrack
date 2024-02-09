using LocationService.Dto;

namespace LocationService.Services;

public interface ILocationsService
{
    Task<ApiResponse<int, Exception>> PostLocation(LocationDto location);
}