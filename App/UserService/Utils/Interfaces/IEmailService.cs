namespace UserService.Utils;

public interface IEmailService
{
    //void EmailSender(string toEmail, string name, string code, string action);
    void VerifyEmail(Guid userId, string code);
    string CodeGenerator(Guid userId);
}