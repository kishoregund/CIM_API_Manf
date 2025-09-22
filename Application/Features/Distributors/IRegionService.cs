using Domain.Entities;

namespace Application.Features.Distributors
{
    public interface IRegionService
    {
        Task<List<Regions>> GetRegionsByDistributorAsync(Guid distributorId);
        Task<List<Regions>> GetRegionsAsync();
        Task<List<Regions>> GetAssignedRegionsAsync();
        Task<Regions> GetRegionAsync(Guid id);
        Task<Guid> CreateRegionAsync(Regions region);
        Task<Guid> UpdateRegionAsync(Regions region);
        Task<bool> DeleteRegionAsync(Guid id);
        Task<bool> IsDuplicateAsync(string distRegName, Guid distId);
    }
}
