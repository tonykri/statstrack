using Microsoft.Extensions.Hosting;
using StatisticsService.Models;
using StatisticsService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BusinessStatsService : BackgroundService
{
    private readonly IStatsService statsService;
    private readonly DataContext dataContext;
    public BusinessStatsService(IStatsService statsService, DataContext dataContext)
    {
        this.statsService = statsService;
        this.dataContext = dataContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DateTime now = DateTime.Now;
        TimeSpan delay;
        if (now.Minute != 0 || now.Second != 0)
            delay = TimeSpan.FromMinutes(60 - now.Minute) - TimeSpan.FromSeconds(now.Second);
        else
            delay = TimeSpan.Zero;

        await Task.Delay(delay, stoppingToken);

        var businesses = dataContext.Businesses
            .Where(b => b.ExpirationDate > DateTime.UtcNow)
            .ToList();
        foreach (var business in businesses)
        {
            statsService.CreateHourlyStatsAsync(business.BusinessId, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var storedBusinesses = dataContext.Businesses
            .Where(b => b.ExpirationDate > DateTime.UtcNow)
            .ToList();
            foreach (var business in storedBusinesses)
            {
                statsService.CreateHourlyStatsAsync(business.BusinessId, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
