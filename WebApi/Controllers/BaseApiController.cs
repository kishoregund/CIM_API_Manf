using Application.Features.Identity.Users;
using Azure;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private ISender _sender = null!;
        public ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    }
}
