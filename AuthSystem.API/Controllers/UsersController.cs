using AuthSystem.Application.DTOs.Users;
using AuthSystem.Application.Interfaces;
using AuthSystem.Security.Permissions;
using AuthSystem.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.ManageUsers)]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var users = await userService.GetUsersAsync(cancellationToken);
        return Ok(new BaseResponse<IReadOnlyCollection<UserSummaryResponse>> 
        { 
            Success = true, 
            Data = users 
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound(new BaseResponse<object> { Success = false, Message = "User not found." });
        }

        return Ok(new BaseResponse<UserDetailResponse> { Success = true, Data = user });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.CreateUserAsync(request, cancellationToken);
        if (user is null)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to create user. Email may already be in use." });
        }

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new BaseResponse<UserDetailResponse> 
        { 
            Success = true, 
            Message = "User created successfully.", 
            Data = user 
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "ID mismatch." });
        }

        var succeeded = await userService.UpdateUserAsync(request, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to update user." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "User updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var succeeded = await userService.DeleteUserAsync(id, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to delete user." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "User deleted successfully." });
    }
}
