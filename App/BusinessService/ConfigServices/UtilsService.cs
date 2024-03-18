using BusinessService.Utils;
using BusinessService.AsymcDataProcessing;
using BusinessService.AsymcDataProcessing.EventHandling;
using BusinessService.AsymcDataProcessing.EventProcessing;
using BusinessService.AsymcDataProcessing.MessageBusClient;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<IPhotoValidator, PhotoValidator>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        services.AddScoped<IEventHandler, BusinessService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}