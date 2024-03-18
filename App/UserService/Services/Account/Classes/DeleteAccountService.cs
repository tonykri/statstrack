using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Dto;
using UserService.Dto.MessageBus.Send;
using UserService.Models;
using UserService.Repositories.Profile;
using UserService.Services.Profile;
using UserService.Utils;

namespace UserService.Services.Account;

public class DeleteAccountService : IDeleteAccountService
{
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly IProfilePhotoService profilePhotoService;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public DeleteAccountService(DataContext dataContext, IEmailService emailService,
            IProfilePhotoService profilePhotoService, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.profilePhotoService = profilePhotoService;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }

    public async Task<ApiResponse<int, Exception>> SendDeletionEmail()
    {
        Guid userId = tokenDecoder.GetUserId();
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        try
        {
            string code = emailService.CodeGenerator(user.Id);
            var message = new EmailNameCodeDto(user.Email, user.FirstName + " " + user.LastName, code, "Delete_Account_Email");
            messageBusClient.Send(ref message);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
        }
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> DeleteAccount(string code)
    {
        Guid userId = tokenDecoder.GetUserId();
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        try
        {
            emailService.VerifyEmail(user.Id, code);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
        }
        await profilePhotoService.DeletePhoto();
        dataContext.Remove(user);
        await dataContext.SaveChangesAsync();

        var message = new UserDeletedDto(userId);
        messageBusClient.Send(ref message);

        return new ApiResponse<int, Exception>(0);
    }
}
