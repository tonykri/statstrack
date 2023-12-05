namespace PaymentService.Utils;

public interface ITokenDecoder
{
    Dictionary<string, string> GetClaims(string jwtToken);
    Guid GetUserId();
    Guid GetUserId(string token);
}