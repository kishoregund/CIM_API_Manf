using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DbInitializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<ApplicationDbContext>(options => options
                     //.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))) 
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient) // service lifetime added as dbcontext errout while inserting masters using json files
                .AddTransient<ITenantDbInitializer, TenantDbInitializer>()
                .AddTransient<ApplicationDbInitializer>();
        }

        public static async Task AddDatabaseInitializerAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ITenantDbInitializer>()
                .InitializeDatabaseAsync(cancellationToken);
        }
    }
}
