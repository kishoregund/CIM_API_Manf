using Application.Features.AMCS.Responses;
using Domain.Entities;

namespace Application.Features.AMCS
{
    public interface IAmcItemsService
    {
        public Task<Guid> CreateAmcItems(AMCItems items);
        public Task<bool> DeleteAmcItems(Guid id);
        Task<AMCItems> GetByIdAsync(Guid requestId);
        Task<Guid> UpdateAsync(AMCItems amcItems);
        Task<List<AmcItemsResponse>> GetByAmcIdAsync(Guid requestAmcId);
    }
}
