using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ProfessionalLifeRepo : IProfessionalLifeRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public ProfessionalLifeRepo(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private string HandleProfessionalLife(ProfessionalLifeDto userData, Guid userId, string action)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            throw new NotFoundException("User not found");
        if(action.Equals("register"))
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
            dataContext.ProfessionalLife.Add(professionalLife);
            user.ProfileStage = ProfileStages.ProfessionalLife.ToString();
        }else
        {
            var professionalLife = dataContext.ProfessionalLife.FirstOrDefault(p => p.UserId == user.Id);
            if(professionalLife is null)
                throw new NotFoundException("User's professional data not found");
            professionalLife.Income = userData.Income;
            professionalLife.WorkingHours = userData.WorkingHours;
            professionalLife.Industry = userData.Industry;
            professionalLife.LevelOfEducation = userData.LevelOfEducation;
        }

        dataContext.SaveChanges();

        return action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
    }

    public string RegisterProfessionalLife(ProfessionalLifeDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return HandleProfessionalLife(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateProfessionalLife(ProfessionalLifeDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            HandleProfessionalLife(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public ProfessionalLife GetProfessionalLife()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var professionalLife = dataContext.ProfessionalLife.FirstOrDefault(p => p.UserId == userId);
            if(professionalLife is null)
                throw new NotFoundException("User data not found");
            return professionalLife;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}