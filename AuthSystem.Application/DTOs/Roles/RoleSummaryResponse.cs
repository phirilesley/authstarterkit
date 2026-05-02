namespace AuthSystem.Application.DTOs.Roles;

public sealed class RoleSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
