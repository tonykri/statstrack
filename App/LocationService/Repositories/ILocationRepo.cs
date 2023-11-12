using LocationService.Dto;

namespace LocationService.Repositories;

public interface ILocationRepo
{
    void PostLocation(LocationDto location);
}