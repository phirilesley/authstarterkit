using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using AuthSystem.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Persistence.Services;

public sealed class EfPermissionReadService(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IPermissionReadService
{
    public async Task<IReadOnlyCollection<string>> GetPermissionsForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return [];
        }

        var roles = await userManager.GetRolesAsync(user);
        
        var permissions = await dbContext.RolePermissions
            .Where(rp => roles.Contains(rp.Role!.Name) && rp.Permission!.IsActive)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission!.Code)
            .Distinct()
            .ToListAsync(cancellationToken);

        return permissions;
    }
}
