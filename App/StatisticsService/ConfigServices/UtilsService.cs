using StatisticsService.AsymcDataProcessing;
using StatisticsService.AsymcDataProcessing.EventHandling;
using StatisticsService.AsymcDataProcessing.EventProcessing;
using StatisticsService.Services;
using StatisticsService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IStatsService, StatsService>();

        services.AddScoped<IEventHandler, StatisticsService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
        services.AddHostedService<BusinessStatsService>(provider =>
        {
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            return new BusinessStatsService(scopeFactory);
        });
        services.AddHostedService(provider =>
        {
            var interval = TimeSpan.FromHours(1);
            var dueTime = TimeSpan.Zero; 

            return new HourlyService(provider, dueTime, interval);
        });
    }
}