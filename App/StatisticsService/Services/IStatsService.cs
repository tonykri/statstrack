namespace StatisticsService.Services;

public interface IStatsService
{
    void CreateHourlyStatsAsync(Guid businessId, DateTime startTime, DateTime endTime);
}