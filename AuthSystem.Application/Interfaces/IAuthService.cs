using AuthSystem.Application.DTOs.Auth;

namespace AuthSystem.Application.Interfaces;

public interface IAuthService
{
    Task<TokenPairResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<TokenPairResponse?> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<RegisterResponse?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
