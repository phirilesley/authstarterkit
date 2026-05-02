using AuthSystem.Application.DTOs.Roles;

namespace AuthSystem.Application.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyCollection<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleSummaryResponse?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleSummaryResponse?> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default);
}
