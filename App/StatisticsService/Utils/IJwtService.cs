namespace StatisticsService.Utils;

public interface IJwtService 
{
    string CreateToken();
    public Guid GetUserId();
}