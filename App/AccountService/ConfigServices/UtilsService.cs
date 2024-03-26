using AccountService.Utils;
using UserService.AsymcDataProcessing;
using UserService.AsymcDataProcessing.MessageBusClient;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddHostedService<MessageBusSubscriber>();
    }
}