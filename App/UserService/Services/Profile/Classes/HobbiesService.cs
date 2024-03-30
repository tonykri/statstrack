using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.MessageBus.Send;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class HobbiesService : IHobbiesService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    private readonly IMessageBusClient messageBusClient;
    public HobbiesService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken, IMessageBusClient messageBusClient)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
        this.messageBusClient = messageBusClient;
    }

    private async Task<ApiResponse<string, Exception>> HandleHobbies(HobbiesDto userData, Guid userId, string action)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        if (action.Equals("update"))
        {
            var hobbies = dataContext.Hobbies.Where(h => h.UserId.ToString().Equals(user.Id.ToString()));
            dataContext.Hobbies.RemoveRange(hobbies);
        }

        foreach (string hobby in userData.Hobbies)
            await dataContext.Hobbies.AddAsync(new Hobby(user, hobby));

        if (action.Equals("register"))
        {
            user.ProfileStage = ProfileStages.Hobbies.ToString();

            var message = new ProfileStageUpdatedDto(user.Id, ProfileStages.Hobbies);
            messageBusClient.Send(ref message);
        }
        await dataContext.SaveChangesAsync();

        string msg = action.Equals("register") ? jwtToken.CreateLoginToken(user) : "Update completed";
        return new ApiResponse<string, Exception>(msg);
    }

    public async Task<ApiResponse<string, Exception>> RegisterHobbies(HobbiesDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "register";
        return await HandleHobbies(userData, userId, action);
    }

    public async Task<ApiResponse<string, Exception>> UpdateHobbies(HobbiesDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "update";
        return await HandleHobbies(userData, userId, action);
    }

}