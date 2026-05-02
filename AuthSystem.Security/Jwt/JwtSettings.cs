namespace AuthSystem.Security.Jwt;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = "AuthSystem";
    public string Audience { get; set; } = "AuthSystem.Client";
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 60;
}
