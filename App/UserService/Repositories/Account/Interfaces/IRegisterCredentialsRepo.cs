using UserService.Dto.Account;

namespace UserService.Repositories.Account;

public interface IRegisterCredentialsRepo
{
    string RegisterCredentials(RegisterCredentialsDto user);
    string RegisterEmailVerify(string code);
}