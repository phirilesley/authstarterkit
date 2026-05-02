using AuthSystem.Application.DTOs.Permissions;
using AuthSystem.Application.Interfaces;
using AuthSystem.Security.Permissions;
using AuthSystem.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.ManageRoles)]
public sealed class PermissionsController(
    IPermissionService permissionService,
    IRolePermissionService rolePermissionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var permissions = await permissionService.GetAllPermissionsAsync(cancellationToken);
        return Ok(new BaseResponse<IReadOnlyCollection<PermissionResponse>>
        {
            Success = true,
            Data = permissions
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var permission = await permissionService.GetPermissionByIdAsync(id, cancellationToken);
        if (permission is null)
        {
            return NotFound(new BaseResponse<object> { Success = false, Message = "Permission not found." });
        }

        return Ok(new BaseResponse<PermissionResponse> { Success = true, Data = permission });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var permission = await permissionService.CreatePermissionAsync(request, cancellationToken);
        if (permission is null)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to create permission. Code may already exist." });
        }

        return CreatedAtAction(nameof(GetById), new { id = permission.Id }, new BaseResponse<PermissionResponse>
        {
            Success = true,
            Message = "Permission created successfully.",
            Data = permission
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "ID mismatch." });
        }

        var succeeded = await permissionService.UpdatePermissionAsync(request, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to update permission." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Permission updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var succeeded = await permissionService.DeletePermissionAsync(id, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to delete permission." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Permission deleted successfully." });
    }

    [HttpGet("roles/{roleId}")]
    public async Task<IActionResult> GetRolePermissions([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var response = await rolePermissionService.GetRolePermissionsAsync(roleId, cancellationToken);
        if (response is null)
        {
            return NotFound(new BaseResponse<object> { Success = false, Message = "Role not found." });
        }

        return Ok(new BaseResponse<RolePermissionsResponse> { Success = true, Data = response });
    }

    [HttpPost("roles/assign")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleRequest request, CancellationToken cancellationToken)
    {
        var succeeded = await rolePermissionService.AssignPermissionToRoleAsync(request, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to assign permission to role." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Permission assigned to role successfully." });
    }

    [HttpDelete("roles/{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RevokePermissionFromRole([FromRoute] Guid roleId, [FromRoute] Guid permissionId, CancellationToken cancellationToken)
    {
        var succeeded = await rolePermissionService.RevokePermissionFromRoleAsync(roleId, permissionId, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to revoke permission from role." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Permission revoked from role successfully." });
    }
}
