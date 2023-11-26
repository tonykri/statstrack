namespace UserService.AsymcDataProcessing.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}