using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ProfessionalLifeRepo : IProfessionalLifeRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    public ProfessionalLifeRepo(ITokenDecoder tokenDecoder, DataContext dataContext)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
    }

    public async Task<ApiResponse<ProfessionalLife, Exception>> GetProfessionalLife()
    {
        Guid userId = tokenDecoder.GetUserId();
        var professionalLife = await dataContext.ProfessionalLife.FirstOrDefaultAsync(p => p.UserId == userId);
        if (professionalLife is null)
            return new ApiResponse<ProfessionalLife, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        return new ApiResponse<ProfessionalLife, Exception>(professionalLife);
    }
}