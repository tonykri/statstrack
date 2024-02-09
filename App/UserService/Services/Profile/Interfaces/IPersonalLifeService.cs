using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Services.Profile;

public interface IPersonalLifeService
{
    Task<ApiResponse<string, Exception>> RegisterPersonalLife(PersonalLifeDto userData);
    Task<ApiResponse<string, Exception>> UpdatePersonalLife(PersonalLifeDto userData);
}