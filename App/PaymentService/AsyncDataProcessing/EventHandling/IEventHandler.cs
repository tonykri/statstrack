namespace PaymentService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void BusinessDeleted(string message);
    void UserDeleted(string message);
}