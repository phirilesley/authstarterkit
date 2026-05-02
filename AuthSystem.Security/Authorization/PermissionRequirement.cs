using Microsoft.AspNetCore.Authorization;

namespace AuthSystem.Security.Authorization;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
