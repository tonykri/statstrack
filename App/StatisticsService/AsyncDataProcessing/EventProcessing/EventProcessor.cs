using System.Text.Json;
using StatisticsService.AsymcDataProcessing.EventHandling;
using StatisticsService.Dto.MessageBus;

namespace StatisticsService.AsymcDataProcessing.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    public EventProcessor(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.BusinessCreated:
                BusinessCreated(message);
                break;
            case EventType.UserDeleted:
                UserDeleted(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);
        if(eventType is null) return EventType.Undetermined;

        switch (eventType.Event)
        {
            case "Business_Deleted":
                Console.WriteLine("--> Business Created Event Detected");
                return EventType.BusinessCreated;
            case "User_Deleted":
                Console.WriteLine("--> User Deleted Event Detected");
                return EventType.UserDeleted;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void BusinessCreated(string message)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            scopedService.BusinessCreated(message);
        }
    }

    private void UserDeleted(string message)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            scopedService.UserDeleted(message);
        }
    }

}

enum EventType
{
    BusinessCreated,
    UserDeleted,
    Undetermined
}