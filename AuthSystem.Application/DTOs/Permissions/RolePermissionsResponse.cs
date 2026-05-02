namespace AuthSystem.Application.DTOs.Permissions;

public sealed class RolePermissionsResponse
{
    public Guid RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public IReadOnlyCollection<PermissionResponse> Permissions { get; init; } = [];
}
