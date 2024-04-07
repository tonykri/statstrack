using BusinessService.Dto;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories;

public class StatsServiceRepo : IStatsServiceRepo
{
    private readonly DataContext dataContext;
    public StatsServiceRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<ApiResponse<BusinessLocationDto, Exception>> GetBusinessLocation(Guid id)
    {
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.Id == id);
        if (business is null)
            return new ApiResponse<BusinessLocationDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        
        return new ApiResponse<BusinessLocationDto, Exception>(new BusinessLocationDto{
            Latitude = business.Latitude,
            Longitude = business.Longitude
        });
    }
}