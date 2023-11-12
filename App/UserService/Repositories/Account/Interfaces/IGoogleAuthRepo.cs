namespace UserService.Repositories.Account;

public interface IGoogleAuthRepo
{
    string SignInGoogle();
    Task<object> SignInGoogleCallback(string code, string state);
}