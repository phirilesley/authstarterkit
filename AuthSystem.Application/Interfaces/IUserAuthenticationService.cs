using AuthSystem.Application.DTOs.Auth;

namespace AuthSystem.Application.Interfaces;

public interface IUserAuthenticationService
{
    Task<AuthenticatedUser?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}
