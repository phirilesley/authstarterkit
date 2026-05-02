using AuthSystem.Domain.Entities;

namespace AuthSystem.Application.Interfaces;

public interface IRefreshTokenStore
{
    Task SaveAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetActiveAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAsync(string token, CancellationToken cancellationToken = default);
}
