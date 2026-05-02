using AuthSystem.Application.Interfaces;

namespace AuthSystem.Auditing.Services;

public sealed class LoginTrackingService : ILoginTrackingService
{
    public Task RecordLoginAttemptAsync(string email, bool success, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
