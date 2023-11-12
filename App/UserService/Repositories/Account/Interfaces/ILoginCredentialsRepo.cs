using UserService.Dto.Account;

namespace UserService.Repositories.Account;

public interface ILoginCredentialsRepo
{
    void SendLoginEmail(string email);
    UserDataDto LoginCredentials(LoginCredentialsDto userData);
}