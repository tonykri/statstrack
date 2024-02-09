using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class HobbiesService : IHobbiesService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public HobbiesService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
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
            user.ProfileStage = ProfileStages.Hobbies.ToString();

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