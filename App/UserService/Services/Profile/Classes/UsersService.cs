using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.MessageBus.Send;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class UsersService : IUsersService
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly JwtToken jwtToken;
    private readonly IMessageBusClient messageBusClient;
    public UsersService(DataContext dataContext, ITokenDecoder tokenDecoder, JwtToken jwtToken, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.jwtToken = jwtToken;
        this.messageBusClient = messageBusClient;
    }

    private async Task<ApiResponse<string, Exception>> HandleUser(UserDto userData, Guid userId, string action)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        user.Birthdate = new DateTime(userData.Birthdate.Year, userData.Birthdate.Month, userData.Birthdate.Day);
        user.Country = userData.Country;
        user.Gender = userData.Gender;
        user.PhoneNumber = "+" + userData.DialingCode + userData.PhoneNumber;

        if (action.Equals("register"))
        {
            user.ProfileStage = ProfileStages.User.ToString();
            var message = new ProfileStageUpdatedDto(user.Id, ProfileStages.User);
            messageBusClient.Send(ref message);
        }
        await dataContext.SaveChangesAsync();

        string msg = action.Equals("register") ? jwtToken.CreateLoginToken(user) : "Update completed";
        return new ApiResponse<string, Exception>(msg);
    }

    public async Task<ApiResponse<string, Exception>> RegisterUser(UserDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "register";
        return await HandleUser(userData, userId, action);
    }

    public async Task<ApiResponse<string, Exception>> UpdateUser(UserDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "update";
        return await HandleUser(userData, userId, action);
    }

}