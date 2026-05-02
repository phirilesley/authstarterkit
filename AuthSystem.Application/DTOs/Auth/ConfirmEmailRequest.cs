namespace AuthSystem.Application.DTOs.Auth;

public sealed class ConfirmEmailRequest
{
    public string Email { get; init; } = string.Empty;
    public string ConfirmationToken { get; init; } = string.Empty;
}
