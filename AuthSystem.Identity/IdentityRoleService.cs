using AuthSystem.Application.DTOs.Roles;
using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class IdentityRoleService(RoleManager<ApplicationRole> roleManager, IAuditService auditService) : IRoleService
{
    public async Task<IReadOnlyCollection<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = roleManager.Roles.ToList();
        var responses = roles.Select(r => new RoleSummaryResponse { Id = r.Id, Name = r.Name ?? string.Empty }).ToList();
        return responses;
    }

    public async Task<RoleSummaryResponse?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return null;
        }

        return new RoleSummaryResponse { Id = role.Id, Name = role.Name ?? string.Empty };
    }

    public async Task<RoleSummaryResponse?> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return null;
        }

        var role = new ApplicationRole { Name = request.Name };
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return null;
        }

        await auditService.RecordAsync("role.create", request.Name, cancellationToken);

        return new RoleSummaryResponse { Id = role.Id, Name = role.Name ?? string.Empty };
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return false;
        }

        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
        {
            return false;
        }

        role.Name = request.Name;
        var result = await roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            await auditService.RecordAsync("role.update", request.Name, cancellationToken);
        }

        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return false;
        }

        var result = await roleManager.DeleteAsync(role);

        if (result.Succeeded)
        {
            await auditService.RecordAsync("role.delete", role.Name ?? string.Empty, cancellationToken);
        }

        return result.Succeeded;
    }
}
