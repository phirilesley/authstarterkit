using AuthSystem.Application.DTOs.Roles;
using AuthSystem.Application.Interfaces;

namespace AuthSystem.Application.Services;

public sealed class RoleService : IRoleService
{
    public Task<IReadOnlyCollection<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<RoleSummaryResponse> roles =
        [
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Admin" }
        ];

        return Task.FromResult(roles);
    }
}
