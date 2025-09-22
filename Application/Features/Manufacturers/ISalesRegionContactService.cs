using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers
{
    public interface ISalesRegionContactService
    {
        Task<List<SalesRegionContact>> GetSalesRegionContactsAsync(Guid SalesRegionId);
        Task<SalesRegionContact> GetSalesRegionContactAsync(Guid id);
        Task<Guid> CreateSalesRegionContactAsync(SalesRegionContact region);
        Task<Guid> UpdateSalesRegionContactAsync(SalesRegionContact region);
        Task<bool> DeleteSalesRegionContactAsync(Guid id);
        Task<bool> IsDuplicateAsync(SalesRegionContactRequest regionContact);
    }
}