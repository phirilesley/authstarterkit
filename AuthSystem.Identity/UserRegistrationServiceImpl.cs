using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class UserRegistrationServiceImpl(UserManager<ApplicationUser> userManager) : IUserRegistrationService
{
    public async Task<UserRegistrationResult> RegisterUserAsync(string email, string password, string fullName, CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return new UserRegistrationResult { Success = false };
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            IsActive = true,
            EmailConfirmed = false
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new UserRegistrationResult { Success = false };
        }

        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

        return new UserRegistrationResult
        {
            Success = true,
            UserId = user.Id,
            ConfirmationToken = confirmationToken
        };
    }

    public async Task<bool> ConfirmEmailAsync(string email, string confirmationToken, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return false;
        }

        var result = await userManager.ConfirmEmailAsync(user, confirmationToken);
        return result.Succeeded;
    }
}
