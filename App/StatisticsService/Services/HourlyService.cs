using StatisticsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace StatisticsService.Services;

public class HourlyService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _interval;

    public HourlyService(IServiceProvider serviceProvider, TimeSpan dueTime, TimeSpan interval)
    {
        _serviceProvider = serviceProvider;
        _dueTime = dueTime;
        _interval = interval;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(_dueTime, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWorkAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var statsService = scope.ServiceProvider.GetRequiredService<IStatsService>();

            var bIds = await dataContext.Businesses
                .Select(b => b.BusinessId)
                .ToListAsync(stoppingToken); // Add Microsoft.EntityFrameworkCore namespace

            foreach (var id in bIds)
            {
                await statsService.CreateHourlyStatsAsync(id, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}