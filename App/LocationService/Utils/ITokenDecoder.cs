namespace LocationService.Utils;

public interface ITokenDecoder
{
    Dictionary<string, string> GetClaims(string jwtToken);
    Guid GetUserId();
}