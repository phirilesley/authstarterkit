using AuthSystem.Application.Interfaces;
using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class UserPasswordService(UserManager<ApplicationUser> userManager) : IUserPasswordService
{
    public async Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return null;
        }

        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> ResetPasswordAsync(string email, string resetToken, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return false;
        }

        var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return false;
        }

        var user = await userManager.FindByIdAsync(parsedUserId.ToString());
        if (user is null)
        {
            return false;
        }

        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }
}
