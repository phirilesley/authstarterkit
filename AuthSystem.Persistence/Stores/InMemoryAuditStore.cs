using System.Collections.Concurrent;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;

namespace AuthSystem.Persistence.Stores;

public sealed class InMemoryAuditStore : IAuditService
{
    private static readonly ConcurrentQueue<AuditLog> Logs = new();

    public Task RecordAsync(string action, string actor, CancellationToken cancellationToken = default)
    {
        Logs.Enqueue(new AuditLog
        {
            Action = action,
            PerformedBy = actor,
            Metadata = string.Empty
        });

        return Task.CompletedTask;
    }
}
