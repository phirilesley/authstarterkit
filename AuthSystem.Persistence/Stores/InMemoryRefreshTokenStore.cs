using System.Collections.Concurrent;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;

namespace AuthSystem.Persistence.Stores;

public sealed class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private static readonly ConcurrentDictionary<string, RefreshToken> Tokens = new(StringComparer.Ordinal);

    public Task SaveAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        Tokens[token.Token] = token;
        return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetActiveAsync(string token, CancellationToken cancellationToken = default)
    {
        if (!Tokens.TryGetValue(token, out var value))
        {
            return Task.FromResult<RefreshToken?>(null);
        }

        if (value.IsRevoked || value.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return Task.FromResult<RefreshToken?>(null);
        }

        return Task.FromResult<RefreshToken?>(value);
    }

    public Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        if (Tokens.TryGetValue(token, out var value))
        {
            value.IsRevoked = true;
            value.RevokedAtUtc = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }
}
