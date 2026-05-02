using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Persistence.Stores;

public sealed class EfRefreshTokenStore(ApplicationDbContext dbContext) : IRefreshTokenStore
{
    public async Task SaveAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        dbContext.RefreshTokens.Add(token);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<RefreshToken?> GetActiveAsync(string token, CancellationToken cancellationToken = default)
        => dbContext.RefreshTokens
            .Where(x => x.Token == token && !x.IsRevoked && x.ExpiresAtUtc > DateTime.UtcNow)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        if (entity is null)
        {
            return;
        }

        entity.IsRevoked = true;
        entity.RevokedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
