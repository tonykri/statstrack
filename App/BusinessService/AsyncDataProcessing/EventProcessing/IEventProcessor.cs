namespace BusinessService.AsymcDataProcessing.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}