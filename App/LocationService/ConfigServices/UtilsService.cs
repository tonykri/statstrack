using LocationService.AsymcDataProcessing;
using LocationService.AsymcDataProcessing.EventHandling;
using LocationService.AsymcDataProcessing.EventProcessing;
using LocationService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();

        services.AddScoped<IEventHandler, LocationService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}