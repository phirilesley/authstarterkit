using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthSystem.Identity;

public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        const string connectionString = "Server=localhost;Database=AuthSystemDb;Trusted_Connection=True;TrustServerCertificate=True";
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new IdentityDbContext(optionsBuilder.Options);
    }
}
