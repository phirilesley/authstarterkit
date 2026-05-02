using AuthSystem.Application.Interfaces;

namespace AuthSystem.Auditing.Services;

public sealed class AuditService : IAuditService
{
    public Task RecordAsync(string action, string actor, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
