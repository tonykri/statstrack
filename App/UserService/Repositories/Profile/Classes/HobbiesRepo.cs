using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class HobbiesRepo : IHobbiesRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public HobbiesRepo(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private string HandleHobbies(HobbiesDto userData, Guid userId, string action)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            throw new NotFoundException("User not found");
            
        if(action.Equals("update"))
        {
            var hobbies = dataContext.Hobbies.Where(h => h.UserId.ToString().Equals(user.Id.ToString()));
            dataContext.Hobbies.RemoveRange(hobbies);
        }

        foreach(string hobby in userData.Hobbies)
            dataContext.Hobbies.Add(new Hobby(user, hobby));

        if(action.Equals("register"))
            user.ProfileStage = ProfileStages.Hobbies.ToString();

        dataContext.SaveChanges();

        return action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
    }

    public string RegisterHobbies(HobbiesDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return HandleHobbies(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateHobbies(HobbiesDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            HandleHobbies(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public ICollection<string> GetHobbies()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            return dataContext.Hobbies
                .Where(h => h.UserId == userId)
                .Select(h => h.UserHobby)
                .ToList();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}