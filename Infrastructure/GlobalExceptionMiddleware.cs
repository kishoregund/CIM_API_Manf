using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class GlobalExceptionMiddleware
    {
        //// code moved to ErrorHandlingMiddleware

        //private readonly RequestDelegate _next;
        //public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

        //public async Task InvokeAsync(HttpContext ctx)
        //{
        //    try
        //    {
        //        await _next(ctx);
        //    }
        //    catch (DbUpdateException dbEx) when (IsUniqueConstraintViolation(dbEx))
        //    {
        //        ctx.Response.StatusCode = StatusCodes.Status409Conflict;
        //        ctx.Response.ContentType = "application/problem+json";
        //        await ctx.Response.WriteAsJsonAsync(new { error = "Duplicate entry detected." });
        //    }
        //    catch (Exception ex)
        //    {
        //        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //        ctx.Response.ContentType = "application/problem+json";
        //        await ctx.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        //    }
        //}

        //private bool IsUniqueConstraintViolation(DbUpdateException ex)
        //{
        //    return ex.InnerException?.Message.ToUpper().Contains("UNIQUE CONSTRAINT") ?? false;
        //}
    }

}
