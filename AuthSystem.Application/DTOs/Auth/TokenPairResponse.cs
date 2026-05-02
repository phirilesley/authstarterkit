namespace AuthSystem.Application.DTOs.Auth;

public sealed class TokenPairResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
}
