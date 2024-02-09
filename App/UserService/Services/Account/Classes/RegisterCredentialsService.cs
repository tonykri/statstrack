using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Account;
using UserService.Dto.MessageBus.Send;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Account;

public class RegisterCredentialsService : IRegisterCredentialsService
{
    private readonly JwtToken jwtToken;
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public RegisterCredentialsService(JwtToken jwtToken, DataContext dataContext, IEmailService emailService, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.jwtToken = jwtToken;
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }
    public async Task<ApiResponse<string, Exception>> RegisterCredentials(RegisterCredentialsDto user)
    {
        try
        {
            if (dataContext.Users.Any(u => u.Email.Equals(user.Email)))
                return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.USER_EXISTS));

            User newUser = new User
            {
                Email = user.Email,
                FullName = user.FullName,
                ProfileStage = ProfileStages.EmailConfirmation.ToString(),
                Provider = "Credentials"
            };
            await dataContext.Users.AddAsync(newUser);
            await dataContext.SaveChangesAsync();

            string code = emailService.CodeGenerator(newUser.Id);
            var message = new EmailNameCodeDto(user.Email, user.FullName, code, "Register_User_Email");
            messageBusClient.Send(ref message);

            string token = jwtToken.CreateLoginToken(newUser);
            return new ApiResponse<string, Exception>(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<string, Exception>(new Exception());
        }
    }

    public async Task<ApiResponse<string, Exception>> RegisterEmailVerify(string code)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            try
            {
                emailService.VerifyEmail(user.Id, code);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            user.ProfileStage = ProfileStages.UserBasics.ToString();
            dataContext.SaveChanges();

            string token = jwtToken.CreateLoginToken(user);
            return new ApiResponse<string, Exception>(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<string, Exception>(new Exception());
        }
    }
}