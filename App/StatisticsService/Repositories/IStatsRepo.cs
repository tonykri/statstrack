using StatisticsService.Dto;

namespace StatisticsService.Repositories;

public interface IStatsRepo
{
    Task<ApiResponse<object, Exception>> GetBusinessStats(Guid businessId, DateTime startTime, DateTime endTime);
}