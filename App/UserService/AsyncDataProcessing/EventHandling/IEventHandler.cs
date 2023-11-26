namespace UserService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void BusinessCreated(string message);
    void BusinessDeleted(string message);
}