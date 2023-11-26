namespace LocationService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void UserDeleted(string message);
}