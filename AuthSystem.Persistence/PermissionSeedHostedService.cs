using AuthSystem.Domain.Entities;
using AuthSystem.Persistence.Context;
using AuthSystem.Security.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSystem.Persistence;

public sealed class PermissionSeedHostedService(IServiceScopeFactory scopeFactory) : IHostedService
{
    private readonly Dictionary<string, string> _defaultPermissions = new()
    {
        // User permissions
        { PermissionConstants.ManageUsers, "Manage all user operations" },
        { PermissionConstants.ViewUsers, "View users list" },
        { PermissionConstants.CreateUsers, "Create new users" },
        { PermissionConstants.EditUsers, "Edit user information" },
        { PermissionConstants.DeleteUsers, "Delete users" },

        // Role permissions
        { PermissionConstants.ManageRoles, "Manage all role operations" },
        { PermissionConstants.ViewRoles, "View roles list" },
        { PermissionConstants.CreateRoles, "Create new roles" },
        { PermissionConstants.EditRoles, "Edit role information" },
        { PermissionConstants.DeleteRoles, "Delete roles" },

        // Permission permissions
        { PermissionConstants.ManagePermissions, "Manage all permission operations" },
        { PermissionConstants.ViewPermissions, "View permissions list" },
        { PermissionConstants.CreatePermissions, "Create new permissions" },
        { PermissionConstants.EditPermissions, "Edit permission information" },
        { PermissionConstants.DeletePermissions, "Delete permissions" },
        { PermissionConstants.AssignPermissions, "Assign permissions to roles" },
    };

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        foreach (var (code, description) in _defaultPermissions)
        {
            if (!await dbContext.Permissions.AnyAsync(p => p.Code == code, cancellationToken))
            {
                var permission = new Permission
                {
                    Code = code,
                    Name = code.Replace(".", " ").ToTitleCase(),
                    Description = description,
                    IsActive = true
                };

                dbContext.Permissions.Add(permission);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public static class StringExtensions
{
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }

        return string.Join(" ", words);
    }
}
