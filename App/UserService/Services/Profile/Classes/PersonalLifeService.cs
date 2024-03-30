using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.MessageBus.Send;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class PersonalLifeService : IPersonalLifeService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    private readonly IMessageBusClient messageBusClient;
    public PersonalLifeService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken, IMessageBusClient messageBusClient)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
        this.messageBusClient = messageBusClient;
    }

    private async Task<ApiResponse<string, Exception>> HandlePersonalLife(PersonalLifeDto userData, Guid userId, string action)
    {
       var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
       if(user is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        if(action.Equals("register"))
        {
            PersonalLife personalLife = new PersonalLife
            {
                UserId = user.Id,
                User = user,
                StayHome = userData.StayHome,
                Married = userData.Married
            };
            user.ProfileStage = ProfileStages.PersonalLife.ToString();
            await dataContext.PersonalLife.AddAsync(personalLife);

            var message = new ProfileStageUpdatedDto(user.Id, ProfileStages.PersonalLife);
            messageBusClient.Send(ref message);
        }else
        {
            var personalLife = dataContext.PersonalLife.FirstOrDefault(p => p.UserId == user.Id);
            if(personalLife is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            personalLife.UserId = user.Id;
            personalLife.User = user;
            personalLife.StayHome = userData.StayHome;
            personalLife.Married = userData.Married;
        }
        
        await dataContext.SaveChangesAsync();

        string msg = action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
        return new ApiResponse<string, Exception>(msg);
    }

    public async Task<ApiResponse<string, Exception>> RegisterPersonalLife(PersonalLifeDto userData)
    {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return await HandlePersonalLife(userData, userId, action);
    }

    public async Task<ApiResponse<string, Exception>> UpdatePersonalLife(PersonalLifeDto userData)
    {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            return await HandlePersonalLife(userData, userId, action);
    }

}