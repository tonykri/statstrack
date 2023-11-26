using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Dto.MessageBus.Send;
using UserService.Models;
using UserService.Repositories.Profile;
using UserService.Utils;

namespace UserService.Repositories.Account;

public class DeleteAccountRepo : IDeleteAccountRepo
{
    private readonly DataContext dataContext;
    private readonly IEmailService emailService;
    private readonly IProfilePhotoRepo profilePhotoRepo;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public DeleteAccountRepo(DataContext dataContext, IEmailService emailService,
            IProfilePhotoRepo profilePhotoRepo, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.emailService = emailService;
        this.profilePhotoRepo = profilePhotoRepo;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }

    public void SendDeletionEmail()
    {
        Guid userId = tokenDecoder.GetUserId();
        var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        try
        {
            string code = emailService.CodeGenerator(user.Id);
            emailService.EmailSender(user.Email, user.FullName, code, "delete");
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void DeleteAccount(string code)
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
        profilePhotoRepo.DeletePhoto();
        dataContext.Remove(user);
        dataContext.SaveChanges();

        messageBusClient.DeleteUser(new UserDeletedDto(userId));
    }
}