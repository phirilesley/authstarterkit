namespace AuthSystem.Infrastructure.Services;

public sealed class EmailService
{
    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
