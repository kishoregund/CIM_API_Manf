namespace Application.Features.Manufacturers
{
    public interface ISalesRegionService
    {
        Task<List<SalesRegion>> GetSalesRegionsAsync(Guid ManufacturerId);
        Task<SalesRegion> GetSalesRegionAsync(Guid id);
        Task<Guid> CreateSalesRegionAsync(SalesRegion SalesRegion);
        Task<Guid> UpdateSalesRegionAsync(SalesRegion SalesRegion);
        Task<bool> DeleteSalesRegionAsync(Guid id);
        Task<bool> IsDuplicateAsync(string salesRegName, Guid manfId);
    }
}