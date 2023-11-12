using UserService.Categories;
using UserService.Dto.Account;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Account;

public class RegisterCredentialsRepo : IRegisterCredentialsRepo
{
    private readonly JwtToken jwtToken;
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly ITokenDecoder tokenDecoder;
    public RegisterCredentialsRepo(JwtToken jwtToken, DataContext dataContext, IEmailService emailService, ITokenDecoder tokenDecoder)
    {
        this.jwtToken = jwtToken;
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.tokenDecoder = tokenDecoder;
    }
    public string RegisterCredentials(RegisterCredentialsDto user)
    {
        try
        {
            if (dataContext.Users.Any(u => u.Email.Equals(user.Email)))
                throw new UserExistsException("User with email " + user.Email + " already exists");

            User newUser = new User
            {
                Email = user.Email,
                FullName = user.FullName,
                ProfileStage = ProfileStages.EmailConfirmation.ToString(),
                Provider = "Credentials"
            };
            dataContext.Users.Add(newUser);
            dataContext.SaveChanges();

            string code = emailService.CodeGenerator(newUser.Id);
            emailService.EmailSender(newUser.Email, newUser.FullName, code, "register");

            return jwtToken.CreateLoginToken(newUser);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public string RegisterEmailVerify(string code)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found");
            try
            {
                emailService.VerifyEmail(user.Id, code);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            user.ProfileStage = ProfileStages.UserBasics.ToString();
            dataContext.SaveChanges();

            return jwtToken.CreateLoginToken(user);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}