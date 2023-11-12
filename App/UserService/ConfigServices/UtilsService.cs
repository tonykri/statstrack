using UserService.Utils;

public static class UtilsService
{
    public static void UtilsConfig(this IServiceCollection services)
    {
        services.AddScoped<ITokenDecoder, TokenDecoder>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPhotoValidator, PhotoValidator>();
        services.AddScoped<JwtToken>();
    }
}