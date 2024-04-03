namespace StatisticsService.Services;

public interface IStatsService
{
    void CreateHourlyStatsAsync(Guid businessId, double businessLat, double businessLong, DateTime startTime, DateTime endTime);
}