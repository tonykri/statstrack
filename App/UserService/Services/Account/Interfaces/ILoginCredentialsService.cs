using UserService.Dto;
using UserService.Dto.Account;

namespace UserService.Services.Account;

public interface ILoginCredentialsService
{
    Task<ApiResponse<int, Exception>> SendLoginEmail(string email);
    Task<ApiResponse<UserDataDto, Exception>> LoginCredentials(LoginCredentialsDto userData);
}