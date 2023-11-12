using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class PersonalLifeRepo : IPersonalLifeRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public PersonalLifeRepo(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private string HandlePersonalLife(PersonalLifeDto userData, Guid userId, string action)
    {
       var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
       if(user is null)
            throw new NotFoundException("User not found");
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
            dataContext.PersonalLife.Add(personalLife);
        }else
        {
            var personalLife = dataContext.PersonalLife.FirstOrDefault(p => p.UserId == user.Id);
            if(personalLife is null)
                throw new NotFoundException("User's personal data not found");
            personalLife.UserId = user.Id;
            personalLife.User = user;
            personalLife.StayHome = userData.StayHome;
            personalLife.Married = userData.Married;
        }
        
        dataContext.SaveChanges();

        return action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
    }

    public string RegisterPersonalLife(PersonalLifeDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return HandlePersonalLife(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdatePersonalLife(PersonalLifeDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            HandlePersonalLife(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public PersonalLife GetPersonalLife()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var personalLife = dataContext.PersonalLife.FirstOrDefault(p => p.UserId == userId);
            if(personalLife is null)
                throw new NotFoundException("User data not found");
            return personalLife;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}