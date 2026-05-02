using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSystem.Identity;

public sealed class IdentitySeedHostedService(
    IServiceScopeFactory scopeFactory,
    IdentitySeedOptions options) : IHostedService
{
    private readonly IdentitySeedOptions _options = options;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync(_options.AdminRole))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = _options.AdminRole });
        }

        var user = await userManager.FindByEmailAsync(_options.AdminEmail);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = _options.AdminEmail,
                Email = _options.AdminEmail,
                FullName = _options.AdminFullName,
                IsActive = true,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, _options.AdminPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to seed admin user: " + string.Join(';', result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, _options.AdminRole))
        {
            await userManager.AddToRoleAsync(user, _options.AdminRole);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
