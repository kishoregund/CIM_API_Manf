
using Application.Features.Customers.Responses;
using Application.Features.UserProfiles.Responses;

namespace Application.Features.Customers
{
    public interface ISiteService
    {
        Task<Site> GetSiteAsync(Guid id);
        Task<List<Site>> GetSitesAsync(Guid customerId);
        Task<List<Site>> GetSitesbyUserIdAsync(Guid customerId);
        Task<List<SiteResponse>> GetSitesByContactAsync(Guid contactId);
        Task<List<UserByContactResponse>> GetSiteUsersAsync(Guid siteId);
        Task<Guid> CreateSiteAsync(Site Site);
        Task<Guid> UpdateSiteAsync(Site Site);
        Task<bool> DeleteSiteAsync(Guid id);
        Task<List<Regions>> GetDistRegionsByCustomerAsync(Guid custId);
        Task<bool> IsDuplicateAsync(string custRegName, Guid custId);
    }
}
