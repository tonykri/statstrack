using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IUserRepo
{
    string RegisterUser(UserDto userData);
    void UpdateUser(UserDto userData);
    User GetUser();
}