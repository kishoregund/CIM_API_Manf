using Application.Exceptions;
using Application.Models.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace WebApi
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (DbUpdateException dbEx) when (IsUniqueConstraintViolation(dbEx))
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                httpContext.Response.ContentType = "application/problem+json";
                await httpContext.Response.WriteAsJsonAsync(new { error = "Duplicate entry detected." });
            }
            catch (DbUpdateException dbEx) when (IsReferenceConstraintViolation(dbEx))
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                httpContext.Response.ContentType = "application/problem+json";
                await httpContext.Response.WriteAsJsonAsync(new { error = "Cannot delete this record – there are existing related items that must be removed first." });
            }
            catch (Exception ex)
            {

                var response = httpContext.Response;
                response.ContentType = "application/json";
                
                var responseWrapper = await ResponseWrapper.FailAsync(ex.Message);

                response.StatusCode = ex switch
                {
                    ConflictException ce => (int)ce.StatusCode,
                    NotFoundException nfe => (int)nfe.StatusCode,
                    ForbiddenException fe => (int)fe.StatusCode,
                    IdentityException ie => (int)ie.StatusCode,
                    UnauthorizedException ue => (int)ue.StatusCode,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(responseWrapper);

                await response.WriteAsync(result);
            }
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
             var isNotUnique = ex.InnerException?.Message.ToUpper().Contains("UNIQUE KEY CONSTRAINT") ?? false;
            if (!isNotUnique)
            {
                isNotUnique = ex.InnerException?.Message.ToUpper().Contains("DUPLICATE KEY") ?? false;
            }
            return isNotUnique;
        }

        private bool IsReferenceConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException?.Message.ToUpper().Contains("REFERENCE CONSTRAINT") ?? false;
        }
        
    }
}
