namespace UserService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void UserRegistered(string message);
    void UserDataUpdated(string message);
    void BusinessCreated(string message);
    void BusinessDeleted(string message);
}