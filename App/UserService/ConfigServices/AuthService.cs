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

        services.AddAuthorizationBuilder()
            .AddPolicy("email_confirmation_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("EmailConfirmation"));
        services.AddAuthorizationBuilder()
            .AddPolicy("user_basics_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("UserBasics"));
        services.AddAuthorizationBuilder()
            .AddPolicy("user_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("User"));
        services.AddAuthorizationBuilder()
            .AddPolicy("professional_life_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("ProfessionalLife"));
        services.AddAuthorizationBuilder()
            .AddPolicy("personal_life_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("PersonalLife"));
        services.AddAuthorizationBuilder()
            .AddPolicy("hobbies_profile_stage", policy => policy.RequireClaim("ProfileStage").Equals("Hobbies"));
    }
}
