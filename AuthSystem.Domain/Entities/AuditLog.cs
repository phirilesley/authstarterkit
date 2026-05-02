using AuthSystem.Shared.Base;

namespace AuthSystem.Domain.Entities;

public sealed class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public string Metadata { get; set; } = string.Empty;
}
