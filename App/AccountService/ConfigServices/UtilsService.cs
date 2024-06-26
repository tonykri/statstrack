using AccountService.AsymcDataProcessing.EventHandling;
using AccountService.AsymcDataProcessing.EventProcessing;
using AccountService.Utils;
using UserService.AsymcDataProcessing;
using UserService.AsymcDataProcessing.MessageBusClient;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IEventHandler, AccountService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        
        services.AddHostedService<MessageBusSubscriber>();
    }
}