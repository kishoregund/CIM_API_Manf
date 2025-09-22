using Application.Features.UserProfiles;
using Application.Features.UserProfiles.Commands;
using Application.Features.UserProfiles.Queries;
using Application.Features.UserProfiles.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserProfilesController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.User_Profile)]
        public async Task<IActionResult> CreateUserProfilesAsync([FromBody] UserProfilesRequest createUserProfiles)
        {
            var response = await Sender.Send(new CreateUserProfilesCommand { UserProfilesRequest = createUserProfiles });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.User_Profile)]
        public async Task<IActionResult> UpdateUserProfilesAsync([FromBody] UserProfilesRequest updateUserProfiles)
        {
            var response = await Sender.Send(new UpdateUserProfilesCommand { UserProfilesRequest = updateUserProfiles });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("delete/{UserProfilesId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.User_Profile)]
        public async Task<IActionResult> DeleteUserProfilesAsync(Guid UserProfilesId)
        {
            var response = await Sender.Send(new DeleteUserProfilesCommand { UserProfilesId = UserProfilesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{UserProfilesId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetUserProfilesByIdAsync(Guid UserProfilesId)
        {
            var response = await Sender.Send(new GetUserProfilesByIdQuery { UserProfilesId = UserProfilesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetUserProfilesAsync()
        {
            var response = await Sender.Send(new GetUserProfilesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        /// Misc 


        [HttpGet("UPregionsbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetRegionsByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetRegionsByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("UPsitesbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetSitesByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetSitesByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("UPdistuserbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetDistUserByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetDistUserByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("UPcustuserbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetSiteUserByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetCustUserByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("UPmanfuserbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.User_Profile)]
        public async Task<IActionResult> GetManfUserByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetManfUserByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
