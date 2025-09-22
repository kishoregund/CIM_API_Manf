using Application.Features.Identity.Tokens.Queries;
using Application.Features.Identity.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.Identity.Users.Queries;
using Namotion.Reflection;
using Infrastructure.OpenApi;
using Application.Models.Wrapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : BaseApiController
    {

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUser([FromBody] TokenRequest tokenRequest)
        {
            IResponseWrapper response = await Sender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);           

        }

    }
}
