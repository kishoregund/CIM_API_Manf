

using Application.Features.UserProfiles.Responses;
using Domain.Views;

namespace Application.Features.UserProfiles

{
    public interface IUserProfilesService
    {
        Task<Domain.Entities.UserProfiles> GetUserProfileEntityAsync(Guid id);
        Task<VW_UserProfile> GetUserProfileAsync(Guid id);
        Task<List<VW_UserProfile>> GetUserProfilesAsync();
        Task<Guid> CreateUserProfilesAsync(Domain.Entities.UserProfiles UserProfiles);
        Task<Guid> UpdateUserProfilesAsync(Domain.Entities.UserProfiles UserProfiles);
        Task<bool> DeleteUserProfilesAsync(Guid id);

        Task<List<Regions>> GetRegionsByContactIdAsync(Guid contactId);
        Task<List<Site>> GetSitesByContactIdAsync(Guid contactId);
        Task<UserByContactResponse> GetDistUserByContactAsync(Guid contactId);
        Task<UserByContactResponse> GetCustUserByContactAsync(Guid contactId);
        Task<UserByContactResponse> GetManfUserByContactAsync(Guid contactId);
    }
}
