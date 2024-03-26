using System.Security.Cryptography;
using AccountService.Dto;
using AccountService.Dto.Response;
using AccountService.Models;
using AccountService.Utils;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly DataContext dataContext;
    private readonly IJwtService jwtService;
    public RefreshTokenService(DataContext dataContext, IJwtService jwtService)
    {
        this.dataContext = dataContext;
        this.jwtService = jwtService;
    }

    public async Task<ApiResponse<AccountDto, Exception>> RefreshToken(string token)
    {
        var storedToken = await dataContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token.Equals(token));
        if (storedToken is null)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        if (storedToken.ExpiringDate < DateTime.UtcNow)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.NOT_VALID));

        var account = await dataContext.Accounts.FirstAsync(a => a.Id == storedToken.AccountId);

        storedToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        storedToken.RefreshDate = DateTime.UtcNow;
        storedToken.ExpiringDate = DateTime.UtcNow.AddDays(7);
        await dataContext.SaveChangesAsync();

        string newToken = jwtService.CreateLoginToken(account);
        var response = new AccountDto
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email,
            Provider = account.Provider,
            ProfileStage = account.ProfileStage,
            RefreshToken = storedToken.Token,
            AccessToken = newToken
        };
        return new ApiResponse<AccountDto, Exception>(response);
    }
}
