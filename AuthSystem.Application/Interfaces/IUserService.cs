using AuthSystem.Application.DTOs.Users;

namespace AuthSystem.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyCollection<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserDetailResponse?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDetailResponse?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}
