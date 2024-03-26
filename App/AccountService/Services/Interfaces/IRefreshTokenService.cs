using AccountService.Dto;
using AccountService.Dto.Response;

namespace AccountService.Services;

public interface IRefreshTokenService 
{
    Task<ApiResponse<AccountDto, Exception>> RefreshToken(string token);
}