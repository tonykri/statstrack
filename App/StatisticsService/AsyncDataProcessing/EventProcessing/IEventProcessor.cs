namespace StatisticsService.AsymcDataProcessing.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}