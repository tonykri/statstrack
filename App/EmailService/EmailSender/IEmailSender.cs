namespace EmailService;

public interface IEmailSender
{
    void Send(string toEmail, string mailBody);
}