namespace AccountService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void UpdateProfileStage(string message);
}