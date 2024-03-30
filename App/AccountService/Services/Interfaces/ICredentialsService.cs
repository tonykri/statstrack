using AccountService.Dto;
using AccountService.Dto.Request;
using AccountService.Dto.Response;

namespace AccountService.Services;

public interface ICredentialsService
{
    Task<ApiResponse<int, Exception>> LoginRequest(string email);
    Task<ApiResponse<AccountDto, Exception>> Login(LoginDto accountData);
    Task<ApiResponse<int, Exception>> Register(RegisterDto accountData);
    Task DeleteRequest();
    Task<ApiResponse<int, Exception>> Delete(string code);
}
