using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class PersonalLifeService : IPersonalLifeService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public PersonalLifeService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
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