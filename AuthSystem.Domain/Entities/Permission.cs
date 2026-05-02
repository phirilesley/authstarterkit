using AuthSystem.Shared.Base;

namespace AuthSystem.Domain.Entities;

public sealed class Permission : BaseEntity
{
    public string Code { get; set; } = string.Empty;
}
