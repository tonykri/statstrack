using AccountService.Dto;
using AccountService.Dto.Response;

namespace AccountService.Services;

public interface IGoogleAuthService
{
    string SignInGoogle();
    Task<ApiResponse<AccountDto, Exception>> SignInGoogleCallback(string code, string state);
}