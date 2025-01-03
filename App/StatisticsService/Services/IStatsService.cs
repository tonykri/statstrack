namespace StatisticsService.Services;

public interface IStatsService
{
    Task CreateHourlyStatsAsync(Guid businessId, DateTime startTime, DateTime endTime);
}