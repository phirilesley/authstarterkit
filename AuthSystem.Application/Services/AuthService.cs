using AuthSystem.Application.DTOs.Auth;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;

namespace AuthSystem.Application.Services;

public sealed class AuthService(
    ITokenService tokenService,
    IAuditService auditService,
    ILoginTrackingService loginTrackingService,
    IRefreshTokenStore refreshTokenStore,
    IPermissionReadService permissionReadService,
    INotificationService notificationService,
    IUserAuthenticationService userAuthenticationService,
    IUserPasswordService userPasswordService) : IAuthService
{
    public async Task<TokenPairResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var user = await userAuthenticationService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);
        var valid = user is not null;

        await loginTrackingService.RecordLoginAttemptAsync(request.Email, valid, cancellationToken);

        if (!valid)
        {
            await notificationService.SendSecurityAlertAsync(request.Email, "Failed login attempt", "A failed login attempt was detected.", cancellationToken);
            return null;
        }

        var permissions = await permissionReadService.GetPermissionsForUserAsync(user!.UserId, cancellationToken);
        var response = await IssueTokenPairAsync(user.UserId, permissions, cancellationToken);

        await auditService.RecordAsync("auth.login", request.Email, cancellationToken);
        return response;
    }

    public async Task<TokenPairResponse?> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return null;
        }

        var activeToken = await refreshTokenStore.GetActiveAsync(request.RefreshToken, cancellationToken);
        if (activeToken is null)
        {
            return null;
        }

        await refreshTokenStore.RevokeAsync(request.RefreshToken, cancellationToken);

        var permissions = await permissionReadService.GetPermissionsForUserAsync(activeToken.UserId, cancellationToken);
        var response = await IssueTokenPairAsync(activeToken.UserId, permissions, cancellationToken);

        await auditService.RecordAsync("auth.refresh", activeToken.UserId, cancellationToken);
        return response;
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        await refreshTokenStore.RevokeAsync(refreshToken, cancellationToken);
        await auditService.RecordAsync("auth.logout", "system", cancellationToken);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return;
        }

        var resetToken = await userPasswordService.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        if (string.IsNullOrWhiteSpace(resetToken))
        {
            return;
        }

        await notificationService.SendSecurityAlertAsync(request.Email, "Password reset request", $"Use this reset token: {resetToken}", cancellationToken);
        await auditService.RecordAsync("auth.forgot-password", request.Email, cancellationToken);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.ResetToken) || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return false;
        }

        var succeeded = await userPasswordService.ResetPasswordAsync(request.Email, request.ResetToken, request.NewPassword, cancellationToken);
        if (succeeded)
        {
            await auditService.RecordAsync("auth.reset-password", request.Email, cancellationToken);
        }

        return succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(request.CurrentPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return false;
        }

        var succeeded = await userPasswordService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, cancellationToken);
        if (succeeded)
        {
            await auditService.RecordAsync("auth.change-password", userId, cancellationToken);
        }

        return succeeded;
    }

    private async Task<TokenPairResponse> IssueTokenPairAsync(string userId, IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        var refreshToken = tokenService.GenerateRefreshToken();
        var expiresAt = tokenService.GetAccessTokenExpiryUtc();

        await refreshTokenStore.SaveAsync(new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            ExpiresAtUtc = expiresAt.AddDays(7)
        }, cancellationToken);

        return new TokenPairResponse
        {
            AccessToken = tokenService.GenerateAccessToken(userId, permissions),
            RefreshToken = refreshToken,
            ExpiresAtUtc = expiresAt
        };
    }
}
