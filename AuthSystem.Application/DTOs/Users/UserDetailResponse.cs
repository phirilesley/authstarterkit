namespace AuthSystem.Application.DTOs.Users;

public sealed class UserDetailResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public bool EmailConfirmed { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
}
