namespace AuthSystem.Security.Permissions;

public sealed class PermissionService
{
    public bool HasPermission(IEnumerable<string> grantedPermissions, string requiredPermission)
        => grantedPermissions.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase);
}
