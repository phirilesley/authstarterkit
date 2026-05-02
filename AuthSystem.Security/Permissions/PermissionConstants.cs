namespace AuthSystem.Security.Permissions;

public static class PermissionConstants
{
    // User Management
    public const string ManageUsers = "users.manage";
    public const string ViewUsers = "users.view";
    public const string CreateUsers = "users.create";
    public const string EditUsers = "users.edit";
    public const string DeleteUsers = "users.delete";

    // Role Management
    public const string ManageRoles = "roles.manage";
    public const string ViewRoles = "roles.view";
    public const string CreateRoles = "roles.create";
    public const string EditRoles = "roles.edit";
    public const string DeleteRoles = "roles.delete";

    // Permission Management
    public const string ManagePermissions = "permissions.manage";
    public const string ViewPermissions = "permissions.view";
    public const string CreatePermissions = "permissions.create";
    public const string EditPermissions = "permissions.edit";
    public const string DeletePermissions = "permissions.delete";
    public const string AssignPermissions = "permissions.assign";
}
