using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Services.Profile;

public interface IUsersService
{
    Task<ApiResponse<string, Exception>> RegisterUser(UserDto userData);
    Task<ApiResponse<string, Exception>> UpdateUser(UserDto userData);
}