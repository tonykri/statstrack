using UserService.Models;

namespace UserService.Utils;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;
    private readonly DataContext dataContext;
    public EmailService(IConfiguration configuration, DataContext dataContext)
    {
        this.configuration = configuration;
        this.dataContext = dataContext;
    }
    

    public void VerifyEmail(Guid userId, string code)
    {

        var user = dataContext.Users.FirstOrDefault(u => u.Id.ToString().Equals(userId.ToString()));
        if(user is null)
            throw new Exception("User not found");
        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.UserId.ToString().Equals(user.Id.ToString()));
        if(emailCode is null)
            throw new Exception("Email's code not found");

        if (emailCode.CodeCreated.AddMinutes(2) < DateTime.Now)
            throw new Exception("Code is not valid after 2 minutes");

        if (!emailCode.Code.Equals(code))
            throw new Exception("Invalid confirmation code");
        
    }

    public string CodeGenerator(Guid userId)
    {
        string characterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        Random random = new Random();

        char[] code = new char[6];
        for (int i = 0; i < 6; i++)
        {
            int index = random.Next(characterSet.Length);
            code[i] = characterSet[index];
        }
        string randomCode = new string(code);

        var user = dataContext.Users.FirstOrDefault(u => u.Id.ToString().Equals(userId.ToString()));
        if(user is null)
            throw new Exception("User not found");

        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.UserId.ToString().Equals(userId.ToString()));
        if (emailCode is not null)
        {
            emailCode.Code = randomCode;
            emailCode.CodeCreated = DateTime.Now;
        }
        else
        {
            dataContext.EmailCodes.Add(new EmailCode
            {
                UserId = user.Id,
                User = user,
                Code = randomCode,
                CodeCreated = DateTime.Now
            });
        }

        dataContext.SaveChanges();

        return randomCode;
    }
}