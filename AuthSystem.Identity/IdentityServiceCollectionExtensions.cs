using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem.Identity;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");
        }

        services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>();

        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IUserPasswordService, UserPasswordService>();

        var seedSection = configuration.GetSection("IdentitySeed");
        var seedOptions = new IdentitySeedOptions
        {
            AdminEmail = seedSection["AdminEmail"] ?? "admin@authsystem.local",
            AdminPassword = seedSection["AdminPassword"] ?? "Pass@123",
            AdminRole = seedSection["AdminRole"] ?? "Admin",
            AdminFullName = seedSection["AdminFullName"] ?? "System Administrator"
        };
        services.AddSingleton(seedOptions);
        services.AddHostedService<IdentitySeedHostedService>();

        return services;
    }
}
