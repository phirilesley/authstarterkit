using AuthSystem.Shared.Base;

namespace AuthSystem.Domain.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}
