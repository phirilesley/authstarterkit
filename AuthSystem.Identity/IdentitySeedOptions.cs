namespace AuthSystem.Identity;

public sealed class IdentitySeedOptions
{
    public string AdminEmail { get; set; } = "admin@authsystem.local";
    public string AdminPassword { get; set; } = "Pass@123";
    public string AdminRole { get; set; } = "Admin";
    public string AdminFullName { get; set; } = "System Administrator";
}
