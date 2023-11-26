using System.Text.Json;
using UserService.AsymcDataProcessing.EventHandling;
using UserService.Dto.MessageBus;

namespace UserService.AsymcDataProcessing.EventProcessing;

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
            case EventType.BusinessDeleted:
                BusinessDeleted(message);
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
            case "Business_Created":
                Console.WriteLine("--> Business Created Event Detected");
                return EventType.BusinessCreated;
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

    private void BusinessDeleted(string message)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            scopedService.BusinessDeleted(message);
        }
    }
}

enum EventType
{
    BusinessCreated,
    BusinessDeleted,
    Undetermined
}