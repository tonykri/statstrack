using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class PersonalLifeRepo : IPersonalLifeRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    public PersonalLifeRepo(ITokenDecoder tokenDecoder, DataContext dataContext)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
    }

    public async Task<ApiResponse<PersonalLife, Exception>> GetPersonalLife()
    {
        Guid userId = tokenDecoder.GetUserId();
        var personalLife = await dataContext.PersonalLife.FirstOrDefaultAsync(p => p.UserId == userId);
        if (personalLife is null)
            return new ApiResponse<PersonalLife, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        return new ApiResponse<PersonalLife, Exception>(personalLife);
    }
}