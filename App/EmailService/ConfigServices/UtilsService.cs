using EmailService;
using EmailService.AsymcDataProcessing;
using EmailService.AsymcDataProcessing.EventHandling;
using EmailService.AsymcDataProcessing.EventProcessing;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
        
        services.AddSingleton<IEventHandler, EmailService.AsymcDataProcessing.EventHandling.EventHandler>();
        services.AddSingleton<IEventProcessor, EventProcessor>();

        services.AddHostedService<MessageBusSubscriber>();
    }
}