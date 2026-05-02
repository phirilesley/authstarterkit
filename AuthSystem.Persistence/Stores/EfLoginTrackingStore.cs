using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;

namespace AuthSystem.Persistence.Stores;

public sealed class EfLoginTrackingStore(ApplicationDbContext dbContext) : ILoginTrackingService
{
    public async Task RecordLoginAttemptAsync(string email, bool success, CancellationToken cancellationToken = default)
    {
        dbContext.LoginAuditLogs.Add(new LoginAuditLog
        {
            Email = email,
            Success = success
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
