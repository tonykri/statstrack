using System.Text.Json;
using DiscountService.AsymcDataProcessing.EventHandling;
using DiscountService.Dto.MessageBus;

namespace DiscountService.AsymcDataProcessing.EventProcessing;

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
            case EventType.BusinessUpdated:
                BusinessUpdated(message);
                break;
            case EventType.BusinessDeleted:
                BusinessDeleted(message);
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
            case "Business_Created":
                Console.WriteLine("--> Business Updated Event Detected");
                return EventType.BusinessCreated;
            case "Business_Updated":
                Console.WriteLine("--> Business Updated Event Detected");
                return EventType.BusinessUpdated;
            case "Business_Deleted":
                Console.WriteLine("--> Business Deleted Event Detected");
                return EventType.BusinessDeleted;
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

    private void BusinessUpdated(string message)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            scopedService.BusinessUpdated(message);
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
    BusinessUpdated,
    BusinessDeleted,
    UserDeleted,
    Undetermined
}