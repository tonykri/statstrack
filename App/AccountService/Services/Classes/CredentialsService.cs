using System.Security.Cryptography;
using AccountService.Categories;
using AccountService.Dto;
using AccountService.Dto.MessageBus;
using AccountService.Dto.Request;
using AccountService.Dto.Response;
using AccountService.Models;
using AccountService.Utils;
using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;

namespace AccountService.Services;

public class CredentialsService : ICredentialsService
{
    private readonly DataContext dataContext;
    private readonly IMessageBusClient messageBusClient;
    private readonly IJwtService jwtService;
    public CredentialsService(DataContext dataContext, IMessageBusClient messageBusClient, IJwtService jwtService)
    {
        this.dataContext = dataContext;
        this.messageBusClient = messageBusClient;
        this.jwtService = jwtService;
    }


    private async Task<string> CodeGenerator(Guid userId)
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

        var user = dataContext.Accounts.First(u => u.Id == userId);

        dataContext.EmailCodes.Add(new EmailCode
        {
            AccountId = user.Id,
            Account = user,
            Code = randomCode
        });

        await dataContext.SaveChangesAsync();
        return randomCode;
    }

    public async Task<ApiResponse<int, Exception>> LoginRequest(string email)
    {
        var account = await dataContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
        if (account is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        string code = await CodeGenerator(account.Id);
        var message = new EmailNameCodeDto(account.Email, account.FirstName + " " + account.LastName, code, "Login_User_Email");
        messageBusClient.Send(ref message);

        dataContext.EmailCodes.Add(new EmailCode
        {
            AccountId = account.Id,
            Account = account,
            Code = code,
            Type = EmailCodeType.Login.ToString()
        });
        await dataContext.SaveChangesAsync();

        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<AccountDto, Exception>> Login(LoginDto accountData, bool emailVerify = false)
    {
        var account = await dataContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(accountData.Email));
        if (account is null)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.Code.Equals(accountData.Code));
        if (emailCode is null)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.NOT_VALID));
        if (emailCode.ExpiresAt < DateTimeOffset.UtcNow || emailCode.IsUsed ||
                emailCode.Type != EmailCodeType.Login.ToString() || emailCode.AccountId != account.Id)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.NOT_VALID));
        emailCode.IsUsed = true;

        if (emailVerify)
            account.ProfileStage = ProfileStages.UserBasics.ToString();

        var refreshToken = new RefreshToken
        {
            AccountId = account.Id,
            Account = account,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
        dataContext.RefreshTokens.Add(refreshToken);
        await dataContext.SaveChangesAsync();

        string token = jwtService.CreateLoginToken(account);
        var response = new AccountDto
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email,
            Provider = account.Provider,
            ProfileStage = account.ProfileStage,
            RefreshToken = refreshToken.Token,
            AccessToken = token
        };
        return new ApiResponse<AccountDto, Exception>(response);
    }

    public async Task<ApiResponse<int, Exception>> Register(RegisterDto accountData)
    {
        var registeredAccount = await dataContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(accountData.Email));
        if (registeredAccount is not null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.ACCOUNT_EXISTS));

        var account = new Account
        {
            FirstName = accountData.FirstName,
            LastName = accountData.LastName,
            Email = accountData.Email
        };
        dataContext.Accounts.Add(account);
        await dataContext.SaveChangesAsync();
        var registerMessage = new UserRegisteredDto(account.Id);
        messageBusClient.Send(ref registerMessage);

        string code = await CodeGenerator(account.Id);
        var message = new EmailNameCodeDto(account.Email, account.FirstName + " " + account.LastName, code, "Register_User_Email");
        messageBusClient.Send(ref message);

        dataContext.EmailCodes.Add(new EmailCode
        {
            AccountId = account.Id,
            Account = account,
            Code = code,
            Type = EmailCodeType.Register.ToString()
        });
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }

    public async Task DeleteRequest()
    {
        var accountId = jwtService.GetUserId();
        var account = await dataContext.Accounts.FirstAsync(a => a.Id == accountId);

        string code = await CodeGenerator(account.Id);
        var message = new EmailNameCodeDto(account.Email, account.FirstName + " " + account.LastName, code, "Delete_Account_Email");
        messageBusClient.Send(ref message);

        dataContext.EmailCodes.Add(new EmailCode
        {
            AccountId = account.Id,
            Account = account,
            Code = code,
            Type = EmailCodeType.Delete.ToString()
        });
        await dataContext.SaveChangesAsync();
    }

    public async Task<ApiResponse<int, Exception>> Delete(string code)
    {
        var accountId = jwtService.GetUserId();
        var account = await dataContext.Accounts.FirstAsync(a => a.Id == accountId);

        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.Code.Equals(code));
        if (emailCode is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_VALID));
        if (emailCode.ExpiresAt < DateTimeOffset.UtcNow || emailCode.IsUsed ||
                emailCode.Type != EmailCodeType.Login.ToString() || emailCode.AccountId != account.Id)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_VALID));
        emailCode.IsUsed = true;

        var message = new UserDeletedDto(accountId);
        messageBusClient.Send(ref message);

        dataContext.Accounts.Remove(account);
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }
}
