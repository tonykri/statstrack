using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class ProfessionalLifeService : IProfessionalLifeService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public ProfessionalLifeService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private async Task<ApiResponse<string, Exception>> HandleProfessionalLife(ProfessionalLifeDto userData, Guid userId, string action)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        if (action.Equals("register"))
        {
            ProfessionalLife professionalLife = new ProfessionalLife
            {
                UserId = user.Id,
                User = user,
                Income = userData.Income,
                WorkingHours = userData.WorkingHours,
                Industry = userData.Industry,
                LevelOfEducation = userData.LevelOfEducation
            };
            await dataContext.ProfessionalLife.AddAsync(professionalLife);
            user.ProfileStage = ProfileStages.ProfessionalLife.ToString();
        }
        else
        {
            var professionalLife = dataContext.ProfessionalLife.FirstOrDefault(p => p.UserId == user.Id);
            if (professionalLife is null)
                return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            professionalLife.Income = userData.Income;
            professionalLife.WorkingHours = userData.WorkingHours;
            professionalLife.Industry = userData.Industry;
            professionalLife.LevelOfEducation = userData.LevelOfEducation;
        }

        await dataContext.SaveChangesAsync();

        string msg = action.Equals("register") ? jwtToken.CreateLoginToken(user) : "Update completed";
        return new ApiResponse<string, Exception>(msg);
    }

    public async Task<ApiResponse<string, Exception>> RegisterProfessionalLife(ProfessionalLifeDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "register";
        return await HandleProfessionalLife(userData, userId, action);
    }

    public async Task<ApiResponse<string, Exception>> UpdateProfessionalLife(ProfessionalLifeDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "update";
        return await HandleProfessionalLife(userData, userId, action);
    }

}