using UserService.Dto;
using UserService.Dto.Account;

namespace UserService.Services.Account;

public interface IRefreshTokenService 
{
    Task<ApiResponse<UserDataDto, Exception>> RefreshToken();
}