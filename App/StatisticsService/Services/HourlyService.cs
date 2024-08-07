using StatisticsService.Models;

namespace StatisticsService.Services;

public class HourlyService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _interval;
    private Timer _timer;

    public HourlyService(IServiceProvider serviceProvider, TimeSpan dueTime, TimeSpan interval)
    {
        _serviceProvider = serviceProvider;
        _dueTime = dueTime;
        _interval = interval;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initialize the timer here
        _timer = new Timer(DoWork, null, _dueTime, _interval);
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        // Prevent overlapping executions
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);

        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var statsService = scope.ServiceProvider.GetRequiredService<IStatsService>();

                var bIds = dataContext.Businesses.Select(b => b.BusinessId).ToList();
                foreach (Guid id in bIds)
                {
                    statsService.CreateHourlyStatsAsync(id, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            // Restart the timer
            _timer?.Change(_interval, _interval);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}


