namespace AuthSystem.Application.DTOs.Users;

public sealed class UpdateUserRequest
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public IEnumerable<string>? RoleNames { get; init; }
}
