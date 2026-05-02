using System.Collections.Concurrent;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;

namespace AuthSystem.Persistence.Stores;

public sealed class InMemoryLoginTrackingStore : ILoginTrackingService
{
    private static readonly ConcurrentQueue<LoginAuditLog> LoginLogs = new();

    public Task RecordLoginAttemptAsync(string email, bool success, CancellationToken cancellationToken = default)
    {
        LoginLogs.Enqueue(new LoginAuditLog
        {
            Email = email,
            Success = success
        });

        return Task.CompletedTask;
    }
}
