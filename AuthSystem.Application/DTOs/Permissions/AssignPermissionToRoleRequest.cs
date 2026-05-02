namespace AuthSystem.Application.DTOs.Permissions;

public sealed class AssignPermissionToRoleRequest
{
    public Guid RoleId { get; init; }
    public Guid PermissionId { get; init; }
}
