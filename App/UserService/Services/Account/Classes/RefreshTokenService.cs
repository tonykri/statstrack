using Microsoft.EntityFrameworkCore;
using UserService.Dto;
using UserService.Dto.Account;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Account;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtToken jwtToken;
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public RefreshTokenService(JwtToken jwtToken, ITokenDecoder tokenDecoder, DataContext dataContext)
    {
        this.jwtToken = jwtToken;
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
    }

    public async Task<ApiResponse<UserDataDto, Exception>> RefreshToken() 
    {
        Guid userId = tokenDecoder.GetUserId();
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<UserDataDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        
        string token = jwtToken.CreateLoginToken(user);
        return new ApiResponse<UserDataDto, Exception>(new UserDataDto(user, token));
    }
}