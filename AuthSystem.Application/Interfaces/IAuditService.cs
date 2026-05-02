namespace AuthSystem.Application.Interfaces;

public interface IAuditService
{
    Task RecordAsync(string action, string actor, CancellationToken cancellationToken = default);
}
