using AuthSystem.Application.DTOs.Auth;
using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class UserAuthenticationService(UserManager<ApplicationUser> userManager) : IUserAuthenticationService
{
    public async Task<AuthenticatedUser?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return null;
        }

        var valid = await userManager.CheckPasswordAsync(user, password);
        if (!valid)
        {
            return null;
        }

        return new AuthenticatedUser
        {
            UserId = user.Id.ToString(),
            Email = user.Email ?? string.Empty
        };
    }
}
