using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;

namespace AuthSystem.Persistence.Stores;

public sealed class EfAuditStore(ApplicationDbContext dbContext) : IAuditService
{
    public async Task RecordAsync(string action, string actor, CancellationToken cancellationToken = default)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            Action = action,
            PerformedBy = actor,
            Metadata = string.Empty
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
