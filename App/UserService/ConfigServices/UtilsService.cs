using UserService.AsymcDataProcessing;
using UserService.AsymcDataProcessing.EventHandling;
using UserService.AsymcDataProcessing.EventProcessing;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<IPhotoValidator, PhotoValidator>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<JwtToken>();

        services.AddScoped<IEventHandler, UserService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}