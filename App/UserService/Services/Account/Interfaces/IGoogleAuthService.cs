namespace UserService.Services.Account;

public interface IGoogleAuthService
{
    string SignInGoogle();
    Task<object> SignInGoogleCallback(string code, string state);
}