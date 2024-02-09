namespace EmailService.AsymcDataProcessing.EventHandling;

public interface IEventHandler
{
    void DeleteAccount(string message);
    void RegisterUser(string message);
    void LoginUser(string message);
}