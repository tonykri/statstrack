using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IProfessionalLifeRepo
{
    string RegisterProfessionalLife(ProfessionalLifeDto userData);
    void UpdateProfessionalLife(ProfessionalLifeDto userData);
    ProfessionalLife GetProfessionalLife();
}