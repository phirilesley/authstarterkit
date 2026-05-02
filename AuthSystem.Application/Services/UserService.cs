using AuthSystem.Application.DTOs.Users;
using AuthSystem.Application.Interfaces;

namespace AuthSystem.Application.Services;

public sealed class UserService : IUserService
{
    public Task<IReadOnlyCollection<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<UserSummaryResponse> users =
        [
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "admin@authsystem.local", FullName = "System Administrator" }
        ];

        return Task.FromResult(users);
    }
}
