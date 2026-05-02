using AuthSystem.Application.DTOs.Users;

namespace AuthSystem.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyCollection<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default);
}
