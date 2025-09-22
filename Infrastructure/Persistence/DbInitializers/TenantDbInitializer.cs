using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.DbInitializers
{
    internal class TenantDbInitializer(TenantDbContext tenantDbContext, IServiceProvider serviceProvider) : ITenantDbInitializer
    {
        private readonly TenantDbContext _tenantDbContext = tenantDbContext;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            await InitializeDatabaseWithTenantAsync(cancellationToken);

            foreach (var tenant in await _tenantDbContext.TenantInfo.Where(x => x.Id != "root").ToListAsync(cancellationToken))
            {
                await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
        }

        private async Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken: cancellationToken)
                is null)
            {
                var rootTenant = new CIMTenantInfo
                {
                    Id = TenancyConstants.Root.Id,
                    Identifier = TenancyConstants.Root.Name,
                    Name = TenancyConstants.Root.Name,
                    AdminEmail = TenancyConstants.Root.Email,
                    IsActive = true,
                    ValidUpTo = DateTime.UtcNow.AddYears(1)
                };

                await _tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task InitializeApplicationDbForTenantAsync(CIMTenantInfo tenant, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            _serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<CIMTenantInfo>()
                {
                    TenantInfo = tenant
                };

            await _serviceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeDatabaseAsync(cancellationToken);
        }

    }
}
