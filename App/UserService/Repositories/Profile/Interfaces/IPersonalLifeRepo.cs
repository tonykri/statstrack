using UserService.Dto.Profile;
using UserService.Models;

namespace UserService.Repositories.Profile;

public interface IPersonalLifeRepo
{
    string RegisterPersonalLife(PersonalLifeDto userData);
    void UpdatePersonalLife(PersonalLifeDto userData);
    PersonalLife GetPersonalLife();
}