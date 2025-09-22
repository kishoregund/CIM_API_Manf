using Application.Features.Distributors.Requests;
using Domain.Entities;

namespace Application.Features.Distributors
{
    public interface IRegionContactService
    {
        Task<List<RegionContact>> GetRegionContactsAsync(Guid regionId);
        Task<RegionContact> GetRegionContactAsync(Guid id);
        Task<Guid> CreateRegionContactAsync(RegionContact region);
        Task<Guid> UpdateRegionContactAsync(RegionContact region);
        Task<bool> DeleteRegionContactAsync(Guid id);
        Task<List<RegionContact>> GetDistributorRegionEngineers(Guid distId, string code);
        Task<List<RegionContact>> GetRegionContactByContact(Guid contactId);
        Task<bool> IsDuplicateAsync(RegionContactRequest regionContact);
    }
}
