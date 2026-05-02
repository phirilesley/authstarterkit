namespace AuthSystem.Application.Interfaces;

public interface INotificationService
{
    Task SendSecurityAlertAsync(string userId, string subject, string message, CancellationToken cancellationToken = default);
}
