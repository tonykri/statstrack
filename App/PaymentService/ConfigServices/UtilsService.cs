using PaymentService.AsymcDataProcessing;
using PaymentService.AsymcDataProcessing.EventHandling;
using PaymentService.AsymcDataProcessing.EventProcessing;
using PaymentService.AsymcDataProcessing.MessageBusClient;
using PaymentService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();

        services.AddScoped<IEventHandler, PaymentService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}