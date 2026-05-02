using AuthSystem.Shared.Base;

namespace AuthSystem.Domain.Entities;

public sealed class LoginAuditLog : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public bool Success { get; set; }
}
