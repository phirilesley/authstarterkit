using AuthSystem.Application.DTOs.Permissions;

namespace AuthSystem.Application.Interfaces;

public interface IRolePermissionService
{
    Task<RolePermissionsResponse?> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> AssignPermissionToRoleAsync(AssignPermissionToRoleRequest request, CancellationToken cancellationToken = default);
    Task<bool> RevokePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
}
