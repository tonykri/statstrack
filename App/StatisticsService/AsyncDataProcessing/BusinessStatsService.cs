using Microsoft.Extensions.Hosting;
using StatisticsService.Models;
using StatisticsService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BusinessStatsService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BusinessStatsService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
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

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var statsService = scope.ServiceProvider.GetRequiredService<IStatsService>();

                var businesses = dataContext.Businesses
                    .Where(b => b.ExpirationDate > DateTime.UtcNow)
                    .ToList();

                foreach (var business in businesses)
                {
                    statsService.CreateHourlyStatsAsync(business.BusinessId, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}

