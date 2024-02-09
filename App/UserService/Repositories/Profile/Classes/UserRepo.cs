using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class UserRepo : IUserRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly JwtToken jwtToken;
    public UserRepo(DataContext dataContext, ITokenDecoder tokenDecoder, JwtToken jwtToken)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.jwtToken = jwtToken;
    }

    public async Task<ApiResponse<User, Exception>> GetUser()
    {
            Guid userId = tokenDecoder.GetUserId();
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user is null)
            return new ApiResponse<User, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            return new ApiResponse<User, Exception>(user);
    }
}