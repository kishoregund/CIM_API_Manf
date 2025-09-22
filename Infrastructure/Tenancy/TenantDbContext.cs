using Application.Features.Identity.Users;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Infrastructure.Persistence.DbConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options)
        : EFCoreStoreDbContext<CIMTenantInfo>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CIMTenantInfo>()
                .ToTable("Tenants", SchemaNames.Multitenancy);

            builder.Entity<UserTenantDto>()
                .ToTable("TenantUsers", SchemaNames.Multitenancy);
        }
    }
}

