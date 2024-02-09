using UserService.Dto;
using UserService.Dto.Account;

namespace UserService.Services.Account;

public interface IRegisterCredentialsService
{
    Task<ApiResponse<string, Exception>> RegisterCredentials(RegisterCredentialsDto user);
    Task<ApiResponse<string, Exception>> RegisterEmailVerify(string code);
}