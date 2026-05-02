namespace AuthSystem.Application.Interfaces;

public interface ILoginTrackingService
{
    Task RecordLoginAttemptAsync(string email, bool success, CancellationToken cancellationToken = default);
}
