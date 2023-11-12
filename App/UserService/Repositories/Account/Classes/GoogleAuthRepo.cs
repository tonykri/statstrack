using Google.Apis.Auth;
using UserService.Categories;
using UserService.Dto.Account;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Account;

public class GoogleAuthRepo : IGoogleAuthRepo
{
    private const string TokenExchangeUrl = "https://oauth2.googleapis.com/token";
    private readonly IConfiguration configuration;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public GoogleAuthRepo(IConfiguration configuration, DataContext dataContext, JwtToken jwtToken)
    {
        this.configuration = configuration;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private async Task<GoogleTokenResponseDto> ExchangeCodeForToken(string code, string clientId, string clientSecret, string redirectUri)
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
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponseDto>(responseContent);
                if(tokenResponse is not null)
                    return tokenResponse;
            }
        }
        return new GoogleTokenResponseDto();
    }

    private GoogleJsonWebSignature.Payload DecodeIdToken(string idToken)
    {
        GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(idToken).Result;
        return payload;
    }

    private async Task<object> SignInGoogleCallbackAsync(string code, string clientId, string clientSecret, string redirectUri)
    {
        var tokenResponse = await ExchangeCodeForToken(code, clientId, clientSecret, redirectUri);
        if(tokenResponse.id_token is null)
            throw new NotFoundException("Token not found");

        GoogleJsonWebSignature.Payload payload = DecodeIdToken(tokenResponse.id_token);
        
        var user = dataContext.Users.FirstOrDefault(u => u.Email.Equals(payload.Email));
        if(user is null)
        {
            User newUser = new User
            {
                Email = payload.Email,
                FullName = payload.Name,
                PhotoUrl = payload.Picture,
                ProfileStage = ProfileStages.UserBasics.ToString(),
                Provider = "Google"
            };
            dataContext.Users.Add(newUser);
            dataContext.SaveChanges();

            return jwtToken.CreateLoginToken(newUser);
        }
        string token = jwtToken.CreateLoginToken(user);
        return new UserDataDto(user, token);
    }
    
    public string SignInGoogle()
    {
        string? clientId = configuration.GetSection("GoogleAuthentication:ClientID").Value;
        string? redirectUri = configuration.GetSection("GoogleAuthentication:RedirectUri").Value;
        string authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=openid%20profile%20email&state=STATE_STRING";
        return authorizationUrl;
    }

    public Task<object> SignInGoogleCallback(string code, string state)
    {
        try
        {
            if (!state.Equals("STATE_STRING"))
            throw new NotFoundException("Error");

            string? clientId = configuration.GetSection("GoogleAuthentication:ClientID").Value;
            string? clientSecret = configuration.GetSection("GoogleAuthentication:ClientSecret").Value;
            string? redirectUri = configuration.GetSection("GoogleAuthentication:RedirectUri").Value;
            if (clientId is null || clientSecret is null || redirectUri is null)
                throw new NotFoundException("Could not find google access data");

            return SignInGoogleCallbackAsync(code, clientId, clientSecret, redirectUri);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}