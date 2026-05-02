using AuthSystem.Application.Interfaces;

namespace AuthSystem.Notifications.Email;

public sealed class EmailSender : INotificationService
{
    public Task SendEmailConfirmationAsync(string email, string confirmationToken, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual email sending with confirmation token
        // For now, this is a placeholder
        return Task.CompletedTask;
    }

    public Task SendWelcomeEmailAsync(string email, string fullName, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual welcome email sending
        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string email, string resetToken, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual password reset email sending
        return Task.CompletedTask;
    }

    public Task SendSecurityAlertAsync(string userId, string subject, string message, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
