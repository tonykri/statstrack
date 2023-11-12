using LocationService.Dto;
using LocationService.Models;
using LocationService.Utils;

namespace LocationService.Repositories;

public class LocationRepo : ILocationRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public LocationRepo(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public void PostLocation(LocationDto location)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            dataContext.Add(new Location{
                UserId = userId,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            });
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}