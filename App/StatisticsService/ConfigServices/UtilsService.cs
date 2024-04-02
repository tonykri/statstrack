using StatisticsService.AsymcDataProcessing;
using StatisticsService.AsymcDataProcessing.EventHandling;
using StatisticsService.AsymcDataProcessing.EventProcessing;
using StatisticsService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler, StatisticsService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}