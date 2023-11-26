namespace ReviewService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void BusinessCreated(string message);
    void BusinessDeleted(string message);
    void BusinessUpdated(string message);
    void UserDeleted(string message);
}