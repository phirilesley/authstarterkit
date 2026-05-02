using AuthSystem.Application.DTOs.Permissions;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Persistence.Services;

public sealed class PermissionService(ApplicationDbContext dbContext, IAuditService auditService) : IPermissionService
{
    public async Task<IReadOnlyCollection<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await dbContext.Permissions
            .Where(p => p.IsActive)
            .Select(p => new PermissionResponse
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);

        return permissions;
    }

    public async Task<PermissionResponse?> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await dbContext.Permissions
            .Where(p => p.Id == id)
            .Select(p => new PermissionResponse
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        return permission;
    }

    public async Task<PermissionResponse?> GetPermissionByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var permission = await dbContext.Permissions
            .Where(p => p.Code == code)
            .Select(p => new PermissionResponse
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        return permission;
    }

    public async Task<PermissionResponse?> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Code) || 
            string.IsNullOrWhiteSpace(request.Name) ||
            await dbContext.Permissions.AnyAsync(p => p.Code == request.Code, cancellationToken))
        {
            return null;
        }

        var permission = new Permission
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };

        dbContext.Permissions.Add(permission);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditService.RecordAsync("permission.create", request.Code, cancellationToken);

        return new PermissionResponse
        {
            Id = permission.Id,
            Code = permission.Code,
            Name = permission.Name,
            Description = permission.Description,
            IsActive = permission.IsActive
        };
    }

    public async Task<bool> UpdatePermissionAsync(UpdatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return false;
        }

        var permission = await dbContext.Permissions.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        if (permission is null)
        {
            return false;
        }

        permission.Name = request.Name;
        permission.Description = request.Description;
        permission.IsActive = request.IsActive;

        dbContext.Permissions.Update(permission);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditService.RecordAsync("permission.update", permission.Code, cancellationToken);

        return true;
    }

    public async Task<bool> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await dbContext.Permissions.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (permission is null)
        {
            return false;
        }

        dbContext.Permissions.Remove(permission);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditService.RecordAsync("permission.delete", permission.Code, cancellationToken);

        return true;
    }
}
