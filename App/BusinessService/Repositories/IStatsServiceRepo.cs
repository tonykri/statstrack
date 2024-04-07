using BusinessService.Dto;

namespace BusinessService.Repositories;

public interface IStatsServiceRepo
{
    Task<ApiResponse<BusinessLocationDto, Exception>> GetBusinessLocation(Guid id);
}