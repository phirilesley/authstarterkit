using AuthSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Identity;

public sealed class IdentityService(UserManager<ApplicationUser> userManager)
{
    public Task<ApplicationUser?> FindByEmailAsync(string email)
        => userManager.FindByEmailAsync(email);
}
