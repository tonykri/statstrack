using UserService.Dto;
using UserService.Dto.Profile;

namespace UserService.Services.Profile;

public interface IHobbiesService
{
    Task<ApiResponse<string, Exception>> RegisterHobbies(HobbiesDto userData);
    Task<ApiResponse<string, Exception>> UpdateHobbies(HobbiesDto userData);
}