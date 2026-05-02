using AuthSystem.Application.DTOs.Roles;
using AuthSystem.Application.Interfaces;
using AuthSystem.Security.Permissions;
using AuthSystem.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.ManageRoles)]
public sealed class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var roles = await roleService.GetRolesAsync(cancellationToken);
        return Ok(new BaseResponse<IReadOnlyCollection<RoleSummaryResponse>> 
        { 
            Success = true, 
            Data = roles 
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var role = await roleService.GetRoleByIdAsync(id, cancellationToken);
        if (role is null)
        {
            return NotFound(new BaseResponse<object> { Success = false, Message = "Role not found." });
        }

        return Ok(new BaseResponse<RoleSummaryResponse> { Success = true, Data = role });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await roleService.CreateRoleAsync(request, cancellationToken);
        if (role is null)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to create role." });
        }

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, new BaseResponse<RoleSummaryResponse> 
        { 
            Success = true, 
            Message = "Role created successfully.", 
            Data = role 
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "ID mismatch." });
        }

        var succeeded = await roleService.UpdateRoleAsync(request, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to update role." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Role updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var succeeded = await roleService.DeleteRoleAsync(id, cancellationToken);
        if (!succeeded)
        {
            return BadRequest(new BaseResponse<object> { Success = false, Message = "Failed to delete role." });
        }

        return Ok(new BaseResponse<object> { Success = true, Message = "Role deleted successfully." });
    }
}
