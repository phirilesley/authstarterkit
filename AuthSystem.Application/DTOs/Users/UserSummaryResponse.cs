namespace AuthSystem.Application.DTOs.Users;

public sealed class UserSummaryResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
}
