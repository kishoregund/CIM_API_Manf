using Application.Features.Identity.Users;
using Application.Features.Identity.Users.Models;
using Azure.Core;
using Infrastructure.Persistence.DbConfigurations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Infrastructure.Identity.Auth
{
    public class CurrentUserMiddleware(ICurrentUserService currentUserService) : IMiddleware
    {
        //private readonly ICurrentUserService _currentUserService = currentUserService;

        //public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        //{
        //    _currentUserService.SetCurrentUser(context.User);
        //    await next(context);
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            currentUserService.SetCurrentUser(context.User);

            var request = context.Request;
            //(new Microsoft.AspNetCore.Routing.RouteValueDictionary.RouteValueDictionaryDebugView(request.RouteValues).Items[0]).Value
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            IReadOnlyDictionary<string, object>? routeValues = ((dynamic)context.Request).RouteValues as IReadOnlyDictionary<string, object>;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

            var routeData = context.GetRouteData();
            string routeValue = string.Empty;
            if (routeData.Values.TryGetValue("action", out object value))   //(request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                if (value.ToString() == "AuthenticateUser")
                {
                    request.EnableBuffering();
                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                    //get body string here...
                    var requestContent = Encoding.UTF8.GetString(buffer);
                    //if (!requestContent.Contains("WebKitFormBoundary"))
                    //{
                    var userTenant = currentUserService.GetLoggedinUserTenant(JsonConvert.DeserializeObject<LoginDto>(requestContent).Email);

                    if (!string.IsNullOrEmpty(userTenant))
                    {
                        request.Headers["Tenant"] = userTenant.ToString();
                    }
                    request.Body.Position = 0;  //rewinding the stream to 0
                    //}
                }
                else if (value.ToString() == "GetToken")
                {
                    request.Headers["Tenant"] = "root";
                }
            }
            await next(context);
        }
    }
}
