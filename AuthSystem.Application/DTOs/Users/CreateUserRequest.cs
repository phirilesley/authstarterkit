namespace AuthSystem.Application.DTOs.Users;

public sealed class CreateUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
    public IEnumerable<string>? RoleNames { get; init; }
}
