using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class HobbiesRepo : IHobbiesRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    public HobbiesRepo(ITokenDecoder tokenDecoder, DataContext dataContext)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
    }

    public async Task<ICollection<string>> GetHobbies()
    {
        Guid userId = tokenDecoder.GetUserId();
        var data = await dataContext.Hobbies
            .Where(h => h.UserId == userId)
            .Select(h => h.UserHobby)
            .ToListAsync();
        return data;
    }
}