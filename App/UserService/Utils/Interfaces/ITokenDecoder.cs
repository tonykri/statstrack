namespace UserService.Utils;

public interface ITokenDecoder
{
    Dictionary<string, string> GetClaims(string jwtToken);
    Guid GetUserId();
}