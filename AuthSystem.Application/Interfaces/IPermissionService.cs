using AuthSystem.Application.DTOs.Permissions;

namespace AuthSystem.Application.Interfaces;

public interface IPermissionService
{
    Task<IReadOnlyCollection<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<PermissionResponse?> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PermissionResponse?> GetPermissionByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<PermissionResponse?> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdatePermissionAsync(UpdatePermissionRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);
}
