using System.Security.Cryptography;
using AccountService.Categories;
using AccountService.Dto;
using AccountService.Dto.ExternalAuth;
using AccountService.Dto.MessageBus;
using AccountService.Dto.Response;
using AccountService.Models;
using AccountService.Utils;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;

namespace AccountService.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private const string TokenExchangeUrl = "https://oauth2.googleapis.com/token";
    private readonly IConfiguration configuration;
    private readonly DataContext dataContext;
    private readonly IJwtService jwtService;
    private readonly IMessageBusClient messageBusClient;
    public GoogleAuthService(IConfiguration configuration, DataContext dataContext, IJwtService jwtService, IMessageBusClient messageBusClient)
    {
        this.configuration = configuration;
        this.dataContext = dataContext;
        this.jwtService = jwtService;
        this.messageBusClient = messageBusClient;
    }

    private async Task<GoogleTokenDto> ExchangeCodeForToken(string code, string clientId, string clientSecret, string redirectUri)
    {
        using (var client = new HttpClient())
        {
            var data = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

            var response = await client.PostAsync(TokenExchangeUrl, new FormUrlEncodedContent(data));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenDto>(responseContent);
                if (tokenResponse is not null)
                    return tokenResponse;
            }
        }
        return new GoogleTokenDto();
    }

    private GoogleJsonWebSignature.Payload DecodeIdToken(string idToken)
    {
        GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(idToken).Result;
        return payload;
    }

    private async Task<AccountDto> HandleLogin(Account account)
    {
        var refreshToken = new RefreshToken
        {
            AccountId = account.Id,
            Account = account,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
        dataContext.RefreshTokens.Add(refreshToken);
        await dataContext.SaveChangesAsync();

        string token = jwtService.CreateLoginToken(account);
        var response = new AccountDto
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email,
            Provider = account.Provider,
            ProfileStage = account.ProfileStage,
            RefreshToken = refreshToken.Token,
            AccessToken = token
        };
        return response;
    }

    private async Task<AccountDto> HandleRegister(GoogleJsonWebSignature.Payload payload)
    {
        Account newUser = new Account
        {
            Email = payload.Email,
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            ProfileStage = ProfileStages.UserBasics.ToString(),
            Provider = "Google"
        };
        dataContext.Accounts.Add(newUser);
        await dataContext.SaveChangesAsync();

        var message = new UserRegisteredDto(newUser.Id, newUser.FirstName, newUser.LastName, newUser.Email)
        {
            ProfileStage = newUser.ProfileStage
        };
        messageBusClient.Send(ref message);

        return await HandleLogin(newUser);
    }

    private async Task<ApiResponse<AccountDto, Exception>> SignInGoogleCallbackAsync(string code, string clientId, string clientSecret, string redirectUri)
    {
        var tokenResponse = await ExchangeCodeForToken(code, clientId, clientSecret, redirectUri);
        if (tokenResponse.id_token is null)
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.GOOGLE_AUTH_FAILED));

        GoogleJsonWebSignature.Payload payload = DecodeIdToken(tokenResponse.id_token);

        var user = await dataContext.Accounts.FirstOrDefaultAsync(u => u.Email.Equals(payload.Email));
        AccountDto data;
        if (user is null)
            data = await HandleRegister(payload);
        else
            data = await HandleRegister(payload);

        return new ApiResponse<AccountDto, Exception>(data);
    }

    public string SignInGoogle()
    {
        string? clientId = configuration.GetSection("GoogleAuthentication:ClientID").Value;
        string? redirectUri = configuration.GetSection("GoogleAuthentication:RedirectUri").Value;
        string authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=openid%20profile%20email&state=STATE_STRING";
        return authorizationUrl;
    }

    public async Task<ApiResponse<AccountDto, Exception>> SignInGoogleCallback(string code, string state)
    {
        if (!state.Equals("STATE_STRING"))
            return new ApiResponse<AccountDto, Exception>(new Exception(ExceptionMessages.GOOGLE_AUTH_FAILED));

        string? clientId = configuration.GetSection("GoogleAuthentication:ClientID").Value;
        string? clientSecret = configuration.GetSection("GoogleAuthentication:ClientSecret").Value;
        string? redirectUri = configuration.GetSection("GoogleAuthentication:RedirectUri").Value;
        if (clientId is null || clientSecret is null || redirectUri is null)
            throw new Exception("Could not find google access data");

        return await SignInGoogleCallbackAsync(code, clientId, clientSecret, redirectUri);
    }
}
