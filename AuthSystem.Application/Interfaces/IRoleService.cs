using AuthSystem.Application.DTOs.Roles;

namespace AuthSystem.Application.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyCollection<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default);
}
