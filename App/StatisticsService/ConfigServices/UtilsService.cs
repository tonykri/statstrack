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
        services.AddHostedService<BusinessStatsService>();
    }
}