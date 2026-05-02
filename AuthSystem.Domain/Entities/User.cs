using AuthSystem.Shared.Base;

namespace AuthSystem.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
