namespace StatisticsService.Utils;

public interface IJwtService 
{
    string CreateLoginToken();
    public Guid GetUserId();
}