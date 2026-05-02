namespace AuthSystem.Application.DTOs.Auth;

public sealed class AuthenticatedUser
{
    public string UserId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
