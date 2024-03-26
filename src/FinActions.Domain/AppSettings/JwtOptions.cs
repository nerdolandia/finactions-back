namespace FinActions.Domain.AppSettings;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecurityKey { get; set; } = null!;
    public int ExpirationInSeconds { get; set; }
}