using AuthSystem.Application.DTOs.Users;
using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class IdentityUserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IAuditService auditService) : IUserService
{
    public async Task<IReadOnlyCollection<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = userManager.Users.ToList();
        var responses = users.Select(u => new UserSummaryResponse 
        { 
            Id = u.Id, 
            Email = u.Email ?? string.Empty,
            FullName = u.FullName ?? string.Empty
        }).ToList();
        return responses;
    }

    public async Task<UserDetailResponse?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        return new UserDetailResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            Roles = roles.ToList()
        };
    }

    public async Task<UserDetailResponse?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            request.Password != request.ConfirmPassword ||
            string.IsNullOrWhiteSpace(request.FullName))
        {
            return null;
        }

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return null;
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            IsActive = request.IsActive,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return null;
        }

        if (request.RoleNames?.Any() == true)
        {
            foreach (var roleName in request.RoleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        await auditService.RecordAsync("user.create", request.Email, cancellationToken);

        return new UserDetailResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            Roles = (await userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task<bool> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return false;
        }

        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return false;
        }

        user.FullName = request.FullName;
        user.IsActive = request.IsActive;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return false;
        }

        if (request.RoleNames is not null)
        {
            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);

            foreach (var roleName in request.RoleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        await auditService.RecordAsync("user.update", user.Email ?? string.Empty, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return false;
        }

        var result = await userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            await auditService.RecordAsync("user.delete", user.Email ?? string.Empty, cancellationToken);
        }

        return result.Succeeded;
    }
}
