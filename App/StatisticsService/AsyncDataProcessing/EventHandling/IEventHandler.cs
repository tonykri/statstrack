namespace StatisticsService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void BusinessCreated(string message);
    void UserDeleted(string message);
}