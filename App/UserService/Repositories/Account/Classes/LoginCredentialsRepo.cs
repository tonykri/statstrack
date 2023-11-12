using UserService.Dto.Account;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Account;

public class LoginCredentialsRepo : ILoginCredentialsRepo
{
    private readonly JwtToken jwtToken;
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly ITokenDecoder tokenDecoder;
    public LoginCredentialsRepo(JwtToken jwtToken, DataContext dataContext, IEmailService emailService, ITokenDecoder tokenDecoder)
    {
        this.jwtToken = jwtToken;
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.tokenDecoder = tokenDecoder;
    }

    public void SendLoginEmail(string email)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Email.Equals(email));
        if (user is null)
            throw new NotFoundException("User with email " + email + " does not exist");

        try
        {
            string code = emailService.CodeGenerator(user.Id);
            emailService.EmailSender(user.Email, user.FullName, code, "login");
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public UserDataDto LoginCredentials(LoginCredentialsDto userData)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Email.Equals(userData.Email));
        if (user is null)
            throw new NotFoundException("User with email " + userData.Email + " does not exist");

        try
        {
            emailService.VerifyEmail(user.Id, userData.Code);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        string token = jwtToken.CreateLoginToken(user);
        return new UserDataDto(user, token);
    }
}