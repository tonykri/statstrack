using System.Text.Json;
using AccountService.Dto.MessageBus;
using AccountService.AsymcDataProcessing.EventHandling;

namespace AccountService.AsymcDataProcessing.EventProcessing;

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
            case EventType.ProfileStageUpdate:
                ProfileStageUpdate(message);
                break;
            default:
                break;
        }
    }

    private void ProfileStageUpdate(string message)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            scopedService.UpdateProfileStage(message);
        }
    }

    private EventType DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);
        if (eventType is null) return EventType.Undetermined;

        switch (eventType.Event)
        {
            case "Profile_Stage_Updated":
                Console.WriteLine("--> Profile Stage Update Event Detected");
                return EventType.ProfileStageUpdate;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }
}

enum EventType
{
    ProfileStageUpdate,
    Undetermined
}