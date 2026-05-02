namespace AuthSystem.Application.DTOs.Roles;

public sealed class UpdateRoleRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
