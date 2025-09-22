
using Application.Features.Customers.Requests;
using MediatR;

namespace Application.Features.Customers
{
    public interface ISiteContactService
    {
        Task<SiteContact> GetSiteContactAsync(Guid id);
        Task<List<SiteContact>> GetSiteContactsAsync(Guid siteId);
        Task<List<SiteContact>> GetSiteContactsByCustomerAsync(Guid customerId);
        Task<Guid> CreateSiteContactAsync(SiteContact SiteContact);
        Task<Guid> UpdateSiteContactAsync(SiteContact SiteContact);
        Task<bool> DeleteSiteContactAsync(Guid id);
        Task<List<SiteContact>> GetSiteContactsByUserIdAsync();
        Task<bool> IsDuplicateAsync(SiteContactRequest siteContact);
    }
}
