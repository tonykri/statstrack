using DiscountService.AsymcDataProcessing;
using DiscountService.AsymcDataProcessing.EventHandling;
using DiscountService.AsymcDataProcessing.EventProcessing;
using DiscountService.AsymcDataProcessing.MessageBusClient;
using DiscountService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();

        services.AddScoped<IEventHandler, DiscountService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}