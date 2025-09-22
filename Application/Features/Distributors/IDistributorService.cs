using Application.Features.Distributors.Responses;
using Domain.Entities;

namespace Application.Features.Distributors
{
    public interface IDistributorService
    {
        Task<DistributorResponse> GetDistributorAsync(Guid id);
        Task<Distributor> GetDistributorEntityAsync(Guid id);        
        Task<List<DistributorResponse>> GetDistributorsAsync();        
        Task<Guid> UpdateDistributorAsync(Distributor mDist);
        Task<Guid> CreateDistributorAsync(Distributor mDist);        
        Task<bool> DeleteDistributorAsync(Guid id);
        Task<List<DistributorResponse>> GetDistributorsByContactAsync(Guid contactId);        
        Task<bool> IsDuplicateAsync(string distributorName);

        //Task<List<RegionContact>> GetDistributorRegionEngineers(Guid id);
        //Task<List<RegionContact>> GetDistributorRegionContacts(Guid id);

        //Task<List<RegionContact>> GetDistributorRegionContactsByContactId(Guid id);
        //Task<int> SaveDistributorTree(DistributorTree mDist, Guid companyId);
    }
}
