namespace Infrastructure.Persistence.DbInitializers
{
    internal interface ITenantDbInitializer
    {
        Task InitializeDatabaseAsync(CancellationToken cancellationToken);
        //Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken);
        //Task InitializeApplicationDbForTenantAsync(CIMTenantInfo tenant, CancellationToken cancellationToken);
    }
}
