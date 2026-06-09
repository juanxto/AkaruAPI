namespace Akaru.API.Auth;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "AkaruAPI";
    public string Audience { get; set; } = "AkaruApp";
    public int ExpirationHours { get; set; } = 24;
}
