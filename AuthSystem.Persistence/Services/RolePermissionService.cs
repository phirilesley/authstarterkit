using AuthSystem.Application.DTOs.Permissions;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Persistence.Services;

public sealed class RolePermissionService(ApplicationDbContext dbContext, IAuditService auditService) : IRolePermissionService
{
    public async Task<RolePermissionsResponse?> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await dbContext.Roles.FindAsync(new object[] { roleId }, cancellationToken: cancellationToken);
        if (role is null)
        {
            return null;
        }

        var permissions = await dbContext.RolePermissions
            .Where(rp => rp.RoleId == roleId && rp.Permission!.IsActive)
            .Include(rp => rp.Permission)
            .Select(rp => new PermissionResponse
            {
                Id = rp.Permission!.Id,
                Code = rp.Permission.Code,
                Name = rp.Permission.Name,
                Description = rp.Permission.Description,
                IsActive = rp.Permission.IsActive
            })
            .ToListAsync(cancellationToken);

        return new RolePermissionsResponse
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Permissions = permissions
        };
    }

    public async Task<bool> AssignPermissionToRoleAsync(AssignPermissionToRoleRequest request, CancellationToken cancellationToken = default)
    {
        var roleExists = await dbContext.Roles.AnyAsync(r => r.Id == request.RoleId, cancellationToken);
        var permissionExists = await dbContext.Permissions.AnyAsync(p => p.Id == request.PermissionId, cancellationToken);

        if (!roleExists || !permissionExists)
        {
            return false;
        }

        var alreadyAssigned = await dbContext.RolePermissions
            .AnyAsync(rp => rp.RoleId == request.RoleId && rp.PermissionId == request.PermissionId, cancellationToken);

        if (alreadyAssigned)
        {
            return false;
        }

        var rolePermission = new RolePermission
        {
            RoleId = request.RoleId,
            PermissionId = request.PermissionId
        };

        dbContext.RolePermissions.Add(rolePermission);
        await dbContext.SaveChangesAsync(cancellationToken);

        var role = await dbContext.Roles.FindAsync(new object[] { request.RoleId }, cancellationToken: cancellationToken);
        var permission = await dbContext.Permissions.FindAsync(new object[] { request.PermissionId }, cancellationToken: cancellationToken);

        await auditService.RecordAsync("role-permission.assign", $"Role: {role?.Name}, Permission: {permission?.Code}", cancellationToken);

        return true;
    }

    public async Task<bool> RevokePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var rolePermission = await dbContext.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

        if (rolePermission is null)
        {
            return false;
        }

        dbContext.RolePermissions.Remove(rolePermission);
        await dbContext.SaveChangesAsync(cancellationToken);

        var role = await dbContext.Roles.FindAsync(new object[] { roleId }, cancellationToken: cancellationToken);
        var permission = await dbContext.Permissions.FindAsync(new object[] { permissionId }, cancellationToken: cancellationToken);

        await auditService.RecordAsync("role-permission.revoke", $"Role: {role?.Name}, Permission: {permission?.Code}", cancellationToken);

        return true;
    }
}
