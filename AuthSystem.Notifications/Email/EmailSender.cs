using AuthSystem.Application.Interfaces;

namespace AuthSystem.Notifications.Email;

public sealed class EmailSender : INotificationService
{
    public Task SendSecurityAlertAsync(string userId, string subject, string message, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
