using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IHobbiesRepo
{
    Task<ICollection<string>> GetHobbies();
}