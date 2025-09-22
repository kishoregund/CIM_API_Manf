using Application.Features.Tenancy;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Tenancy
{
    internal static class TenancyServiceExtensions
    {
        internal static IServiceCollection AddMultitenancyServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<TenantDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))                    
                //.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                //    sqlOptions => sqlOptions.EnableRetryOnFailure(
                //        maxRetryCount: 5,           // Optional: Number of retry attempts
                //        maxRetryDelay: TimeSpan.FromSeconds(10), // Optional: Delay between retries
                //        errorNumbersToAdd: null     // Optional: Specific SQL error numbers to retry on
                //    )))
                .AddMultiTenant<CIMTenantInfo>()
                    .WithHeaderStrategy(TenancyConstants.TenantIdName)
                    .WithClaimStrategy(ClaimConstants.Tenant)
                    //.WithCustomQueryStringStrategy(TenancyConstants.TenantIdName)
                    .WithEFCoreStore<TenantDbContext, CIMTenantInfo>()
                    .Services
                    .AddScoped<ITenantService, TenantService>();
        } 

        internal static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            return app
                .UseMultiTenant();
        }

        //private static FinbuckleMultiTenantBuilder<CIMTenantInfo> WithCustomQueryStringStrategy( 
        //    this FinbuckleMultiTenantBuilder<CIMTenantInfo> builder, string customQueryStringStrategy)
        //{
        //    return builder
        //        .WithDelegateStrategy(context =>
        //        {
        //            if (context is not HttpContext httpContext)
        //            {
        //                return Task.FromResult((string)null);
        //            }

        //            httpContext.Request.Query.TryGetValue(customQueryStringStrategy, out StringValues tenantIdParam);

        //            return Task.FromResult(tenantIdParam.ToString());
        //        });
        //}
    }
}
