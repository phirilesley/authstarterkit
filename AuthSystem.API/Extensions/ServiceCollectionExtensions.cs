using AuthSystem.Application.Interfaces;
using AuthSystem.Application.Services;
using AuthSystem.Identity;
using AuthSystem.Notifications.Email;
using AuthSystem.Persistence;
using AuthSystem.Persistence.Stores;
using AuthSystem.Security.Authorization;
using AuthSystem.Security.Jwt;
using AuthSystem.Security.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthSystem.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddPersistence(configuration);
        services.AddIdentityModule(configuration);

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IAuditService, EfAuditStore>();
        services.AddScoped<ILoginTrackingService, EfLoginTrackingStore>();
        services.AddScoped<IRefreshTokenStore, EfRefreshTokenStore>();
        services.AddSingleton<IPermissionReadService, InMemoryPermissionReadService>();
        services.AddScoped<INotificationService, EmailSender>();

        var jwtSettings = new JwtSettings();
        configuration.GetSection("Jwt").Bind(jwtSettings);

        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey must be at least 32 characters.");
        }

        services.AddSingleton(jwtSettings);
        services.AddSingleton<ITokenService, JwtTokenGenerator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PermissionConstants.ManageUsers,
                policy => policy.Requirements.Add(new PermissionRequirement(PermissionConstants.ManageUsers)));
            options.AddPolicy(PermissionConstants.ManageRoles,
                policy => policy.Requirements.Add(new PermissionRequirement(PermissionConstants.ManageRoles)));
        });

        return services;
    }
}
