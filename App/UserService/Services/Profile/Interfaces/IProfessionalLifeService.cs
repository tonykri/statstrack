using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Services.Profile;

public interface IProfessionalLifeService
{
    Task<ApiResponse<string, Exception>> RegisterProfessionalLife(ProfessionalLifeDto userData);
    Task<ApiResponse<string, Exception>> UpdateProfessionalLife(ProfessionalLifeDto userData);
}