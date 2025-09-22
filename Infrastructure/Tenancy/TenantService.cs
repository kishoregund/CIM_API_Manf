using Application.Features.Identity.Users;
using Application.Features.Tenancy;
using Application.Features.Tenancy.Models;
using Application.Models;
using Azure.Core;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Migrations.TenantDb;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DbInitializers;
using Infrastructure.Services;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Infrastructure.Tenancy
{
    public class TenantService(IMultiTenantStore<CIMTenantInfo> tenantStore,
        ApplicationDbInitializer applicationDbInitializer, IUserService userService, IConfiguration configuration,
        IServiceProvider serviceProvider) : ITenantService
    {
        private readonly ApplicationDbInitializer _applicationDbInitializer = applicationDbInitializer;
        private readonly IUserService _userService = userService;

        public async Task<string> ActivateAsync(string id)
        {
            var tenantInDb = await tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = true;

            await tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct)
        {
            var appSetting = configuration.GetSection("AppSettings").Get<AppSettings>();
            // Create tenant in db
            var newTenant = new CIMTenantInfo
            {
                Id = createTenant.Identifier,
                Identifier = createTenant.Identifier.Replace(" ", ""),
                Name = createTenant.Name,
                ConnectionString = @"Data Source=" + appSetting.DBServerName + "; Initial Catalog=" + createTenant.ConnectionString + ";user id=" + appSetting.DBUserId + ";password=" + appSetting.DBPassword + ";TrustServerCertificate=True; MultipleActiveResultSets=True;",
                AdminEmail = createTenant.AdminEmail,
                ValidUpTo = createTenant.ValidUpTo,
                IsActive = createTenant.IsActive
            };

            await tenantStore.TryAddAsync(newTenant);

            // Initialize tenant with Users, User Roles, Roles and Role Permissions
            // Admin user should be added in the tenantusers root db 
            var tenantUser = new UserTenantDto
            {
                Id = Guid.NewGuid(),
                Email = createTenant.AdminEmail,
                TenantId = createTenant.Identifier  // tenantid should be loggedin tenant
            };
            await _userService.CreateTenantUserAsync(tenantUser);

            try
            {
                using var scope = serviceProvider.CreateScope();
                serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
                    .MultiTenantContext = new MultiTenantContext<CIMTenantInfo>()
                    {
                        TenantInfo = newTenant
                    };
                await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                    .InitializeDatabaseAsync(ct);
            }
            catch (Exception ex)
            {
                await tenantStore.TryRemoveAsync(createTenant.Identifier);
                throw;
            }

            return newTenant.Id;
        }

        public async Task<string> DeactivateAsync(string id)
        {
            var tenantInDb = await tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = false;

            await tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<TenantDto> GetTenantByIdAsync(string id)
        {
            var tenantInDb = await tenantStore.TryGetAsync(id);

            #region Manual Mapping - Opt 1
            return new TenantDto()
            {
                Id = tenantInDb.Id,
                Identifier = tenantInDb.Identifier,
                Name = tenantInDb.Name,
                AdminEmail = tenantInDb.AdminEmail,
                ConnectionString = tenantInDb.ConnectionString,
                ValidUpTo = tenantInDb.ValidUpTo.Date.ToString("dd/MM/yyyy"),
                IsActive = tenantInDb.IsActive
            };
            #endregion

            // Using a mapping library - Opt 2 (Best)
            //return tenantInDb.Adapt<TenantDto>();
        }

        public async Task<List<TenantDto>> GetTenantsAsync()
        {
            var tenantsInDb = await tenantStore.GetAllAsync();
            tenantsInDb = tenantsInDb.Where(x => x.Id.ToLower() != "root");
            List<TenantDto> tenants = new List<TenantDto>();
            foreach (CIMTenantInfo tenant in tenantsInDb)
            {
                tenants.Add(new TenantDto()
                {
                    Id = tenant.Id,
                    Identifier = tenant.Identifier,
                    Name = tenant.Name,
                    AdminEmail = tenant.AdminEmail,
                    ConnectionString = tenant.ConnectionString,
                    ValidUpTo = tenant.ValidUpTo.Date.ToString("dd/MM/yyyy"),
                    IsActive = tenant.IsActive
                });
            }

            return tenants.ToList();
        }

        public async Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate)
        {
            var tenantInDb = await tenantStore.TryGetAsync(id);
            tenantInDb.ValidUpTo = newExpiryDate;

            await tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }


    }
}
