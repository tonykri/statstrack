using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IProfessionalLifeRepo
{
    Task<ApiResponse<ProfessionalLife, Exception>> GetProfessionalLife();
}