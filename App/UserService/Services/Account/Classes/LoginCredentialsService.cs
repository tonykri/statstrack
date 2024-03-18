using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Dto;
using UserService.Dto.Account;
using UserService.Dto.MessageBus.Send;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Account;

public class LoginCredentialsService : ILoginCredentialsService
{
    private readonly JwtToken jwtToken;
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly IMessageBusClient messageBusClient;
    public LoginCredentialsService(JwtToken jwtToken, DataContext dataContext, IEmailService emailService, IMessageBusClient messageBusClient)
    {
        this.jwtToken = jwtToken;
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.messageBusClient = messageBusClient;
    }

    public async Task<ApiResponse<int, Exception>> SendLoginEmail(string email)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        if (user is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        try
        {
            string code = emailService.CodeGenerator(user.Id);
            var message = new EmailNameCodeDto(user.Email, user.FirstName + " " + user.LastName, code, "Login_User_Email");
            messageBusClient.Send(ref message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
        }
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<UserDataDto, Exception>> LoginCredentials(LoginCredentialsDto userData)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(userData.Email));
        if (user is null)
            return new ApiResponse<UserDataDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        try
        {
            emailService.VerifyEmail(user.Id, userData.Code);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<UserDataDto, Exception>(new Exception());
        }

        string token = jwtToken.CreateLoginToken(user);
        return new ApiResponse<UserDataDto, Exception>(new UserDataDto(user, token));
    }
}