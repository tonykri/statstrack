namespace BusinessService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void BusinessCreated(string message);
    void UserDeleted(string message);
    void ReviewsUpdated(string message);
}