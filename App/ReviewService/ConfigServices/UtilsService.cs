using ReviewService.AsymcDataProcessing;
using ReviewService.AsymcDataProcessing.EventHandling;
using ReviewService.AsymcDataProcessing.EventProcessing;
using ReviewService.AsymcDataProcessing.MessageBusClient;
using ReviewService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();

        services.AddScoped<IEventHandler, ReviewService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddScoped<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}