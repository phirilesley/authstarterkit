using System.Security.Claims;
using AuthSystem.Application.DTOs.Auth;
using AuthSystem.Application.Interfaces;
using AuthSystem.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        if (result is null)
        {
            return Unauthorized(new BaseResponse<object> { Success = false, Message = "Invalid credentials." });
        }

        return Ok(new BaseResponse<TokenPairResponse> { Success = true, Message = "Login successful.", Data = result });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);
        if (result is null)
        {
            return Unauthorized(new BaseResponse<object> { Success = false, Message = "Invalid refresh token." });
        }

        return Ok(new BaseResponse<TokenPairResponse> { Success = true, Message = "Token refreshed.", Data = result });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(request.RefreshToken, cancellationToken);
        return Ok(new BaseResponse<object> { Success = true, Message = "Logged out." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await authService.ForgotPasswordAsync(request, cancellationToken);
        return Ok(new BaseResponse<object> { Success = true, Message = "If the account exists, a reset instruction was sent." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var succeeded = await authService.ResetPasswordAsync(request, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Password reset failed." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Password has been reset." });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? string.Empty;
        var succeeded = await authService.ChangePasswordAsync(userId, request, cancellationToken);

        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Password change failed." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Password changed successfully." });
    }
}
