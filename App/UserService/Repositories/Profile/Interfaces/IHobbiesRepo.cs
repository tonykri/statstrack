using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IHobbiesRepo
{
    string RegisterHobbies(HobbiesDto userData);
    void UpdateHobbies(HobbiesDto userData);
    ICollection<string> GetHobbies();
}