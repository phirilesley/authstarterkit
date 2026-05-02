namespace AuthSystem.Application.Interfaces;

public interface IPermissionReadService
{
    Task<IReadOnlyCollection<string>> GetPermissionsForUserAsync(string userId, CancellationToken cancellationToken = default);
}
