namespace AuthSystem.Application.Interfaces;

public interface INotificationService
{
    Task SendEmailConfirmationAsync(string email, string confirmationToken, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string email, string fullName, CancellationToken cancellationToken = default);
    Task SendPasswordResetAsync(string email, string resetToken, CancellationToken cancellationToken = default);
    Task SendSecurityAlertAsync(string userId, string subject, string message, CancellationToken cancellationToken = default);
}
