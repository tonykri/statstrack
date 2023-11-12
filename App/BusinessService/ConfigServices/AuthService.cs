using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthService
{
    public static void AuthConfig(this IServiceCollection services, string? AppToken)
    {
        if(AppToken is null) 
            return;
            
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(AppToken)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        services.AddAuthorization();
        services.AddAuthorizationBuilder()
            .AddPolicy("completed_profile", policy => policy.RequireClaim("ProfileStage").Equals("Completed"));
    }
}