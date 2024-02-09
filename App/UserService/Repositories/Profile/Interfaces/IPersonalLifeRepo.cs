using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IPersonalLifeRepo
{
    Task<ApiResponse<PersonalLife, Exception>> GetPersonalLife();
}