using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace StatisticsService.Utils;

public class JwtService : IJwtService
{
    private readonly IConfiguration configuration;
    private IHttpContextAccessor httpContextAccessor;

    public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        this.configuration = configuration;
        this.httpContextAccessor = httpContextAccessor;
    }

    private Dictionary<string, string> GetClaims(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);
        var claims = token.Claims;

        Dictionary<string, string> hashMap = new Dictionary<string, string>();

        foreach (var claim in claims)
        {
            hashMap[claim.Type] = claim.Value;
        }
        return hashMap;
    }

    public string CreateLoginToken()
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "StatisticsService")
        };

        string? AppToken = configuration.GetSection("AppSettings:Token").Value;
        if (AppToken is null)
            throw new Exception("AppSettings:Token not found in configuration");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppToken));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }


    public Guid GetUserId()
    {
        var givenToken = httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
        if (givenToken is null)
            throw new InvalidDataException("Invalid token");
        givenToken = givenToken.Split(" ").Last();
        Dictionary<string, string> claims = GetClaims(givenToken);

        return Guid.Parse(claims[ClaimTypes.Sid]);
    }

}