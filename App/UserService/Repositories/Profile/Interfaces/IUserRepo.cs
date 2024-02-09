using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IUserRepo
{
    Task<ApiResponse<User, Exception>> GetUser();
}