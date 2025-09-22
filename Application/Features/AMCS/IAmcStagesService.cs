using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS
{
    public interface IAmcStagesService
    {
        public Task<Guid> CreateAmcStages(AMCStages amcItems);
        public Task<bool> DeleteAmcStages(Guid id);
        public Task<AMCStages> GetByIdAsync(Guid requestId);
        public Task<List<AmcStagesResponse>> GetAllByAmcIdAsync(Guid amcId);
        Task<Guid> UpdateAsync(AMCStages amcStages);
    }
}