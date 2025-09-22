using Application.Features.Identity.Roles;
using Application.Features.Identity.Roles.Commands;
using Application.Features.Identity.Roles.Queries;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RolesController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Role)]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRolePermissionRequest createRole)
        {
            var response = await Sender.Send(new CreateRoleCommand { RoleRequest = createRole });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Role)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest updateRole)
        {
            var response = await Sender.Send(new UpdateRoleCommand { UpdateRole = updateRole });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("updatepermissions")]
        [ShouldHavePermission(CimAction.Update, CimFeature.RoleClaims)]
        public async Task<IActionResult> UpdateRoleClaimsAsync([FromBody] UpdateRolePermissionsRequest updateRoleClaims)
        {
            var response = await Sender.Send(new UpdateRolePermissionsCommand { UpdateRolePermissions = updateRoleClaims });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{roleId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Role)]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var response = await Sender.Send(new DeleteRoleCommand { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Role)]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await Sender.Send(new GetRolesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("role/{roleId}")]
        //[ShouldHavePermission(CimAction.View, CimFeature.Role)] /// commented as the distributor user does not have access
        public async Task<IActionResult> GetPartialRoleByIdAsync(string roleId)
        {
            var response = await Sender.Send(new GetRoleByIdQuery { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("allscreens")]
        [ShouldHavePermission(CimAction.View, CimFeature.Role)]
        public async Task<IActionResult> GetAllScreensAsync()
        {
            var response = await Sender.Send(new GetAllScreensQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[HttpGet("full/{roleId}")]
        //[ShouldHavePermission(CimAction.View, CimFeature.Roles)]
        //public async Task<IActionResult> GetDetailedRoleByIdAsync(string roleId)
        //{
        //    var response = await Sender.Send(new GetRoleWithPermissionsQuery { RoleId = roleId });
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }
        //    return BadRequest(response);
        //}
    }
}
