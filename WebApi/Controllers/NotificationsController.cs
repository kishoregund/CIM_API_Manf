using Application.Features.Notifications.Commands;
using Application.Features.Notifications.Queries;
using Application.Features.Notifications.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NotificationsController : BaseApiController
    {
        [HttpPost("add")]
        //[ShouldHavePermission(CimAction.Create, CimFeature.Notifications)]
        public async Task<IActionResult> CreateNotificationAsync([FromBody] NotificationsRequest createNotification)
        {
            var response = await Sender.Send(new CreateNotificationsCommand { NotificationsRequest = createNotification });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
               

        [HttpDelete("delete/{NotificationId}")]
        //[ShouldHavePermission(CimAction.Delete, CimFeature.Notifications)]
        public async Task<IActionResult> DeleteNotificationAsync(Guid NotificationId)
        {
            var response = await Sender.Send(new DeleteNotificationsCommand { NotificationsId = NotificationId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("clear")]
        //[ShouldHavePermission(CimAction.Delete, CimFeature.Notifications)]
        public async Task<IActionResult> DeleteNotificationByUserAsync()
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var LoggedInUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await Sender.Send(new DeleteNotificationsByUserCommand { UserId = Guid.Parse(LoggedInUserId) });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        //[ShouldHavePermission(CimAction.View, CimFeature.Notifications)]
        public async Task<IActionResult> GetNotificationsAsync()
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var LoggedInUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await Sender.Send(new GetNotificationsQuery { UserId = Guid.Parse(LoggedInUserId) });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
