using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class UserRepo : IUserRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly JwtToken jwtToken;
    public UserRepo(DataContext dataContext, ITokenDecoder tokenDecoder, JwtToken jwtToken)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.jwtToken = jwtToken;
    }

    private string HandleUser(UserDto userData, Guid userId, string action)
    {   
        var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            throw new NotFoundException("User not found");
        user.Birthdate = new DateTime(userData.Birthdate.Year, userData.Birthdate.Month, userData.Birthdate.Day);
        user.Country = userData.Country;
        user.Gender = userData.Gender;
        user.PhoneNumber = "+" + userData.DialingCode + userData.PhoneNumber;

        if(action.Equals("register"))
            user.ProfileStage = ProfileStages.User.ToString();

        dataContext.SaveChanges();

        return action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
    }

    public string RegisterUser(UserDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return HandleUser(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateUser(UserDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            HandleUser(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public User GetUser()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if(user is null)
                throw new NotFoundException("User not found");
            return user;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}