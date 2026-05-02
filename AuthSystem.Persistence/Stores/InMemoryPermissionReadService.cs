using AuthSystem.Application.Interfaces;
using AuthSystem.Security.Permissions;

namespace AuthSystem.Persistence.Stores;

public sealed class InMemoryPermissionReadService : IPermissionReadService
{
    public Task<IReadOnlyCollection<string>> GetPermissionsForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<string> permissions =
        [
            PermissionConstants.ManageUsers,
            PermissionConstants.ManageRoles
        ];

        return Task.FromResult(permissions);
    }
}
