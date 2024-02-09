using System.Text.Json;
using EmailService.AsymcDataProcessing.EventHandling;
using EmailService.Dto;

namespace EmailService.AsymcDataProcessing.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IEventHandler eventHandler;
    public EventProcessor(IEventHandler eventHandler)
    {
        this.eventHandler = eventHandler;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.DeleteAccount:
                DeleteAccount(message);
                break;
            case EventType.RegisterUser:
                RegisterUser(message);
                break;
            case EventType.LoginUser:
                LoginUser(message);
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
            case "Delete_Account_Email":
                Console.WriteLine("--> Delete Account Event Detected");
                return EventType.DeleteAccount;
            case "Register_User_Email":
                Console.WriteLine("--> Register User Event Detected");
                return EventType.RegisterUser;
            case "Login_User_Email":
                Console.WriteLine("--> Login User Event Detected");
                return EventType.LoginUser;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    
    private void DeleteAccount(string message)
    {
        eventHandler.DeleteAccount(message);
    }

    private void RegisterUser(string message)
    {
        eventHandler.RegisterUser(message);
    }

    private void LoginUser(string message)
    {
        eventHandler.LoginUser(message);
    }

}

enum EventType
{
    DeleteAccount,
    LoginUser,
    RegisterUser,
    Undetermined
}