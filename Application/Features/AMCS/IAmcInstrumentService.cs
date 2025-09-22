using Application.Features.AMCS.Queries;
using Application.Features.AMCS.Requests;
using Application.Features.AMCS.Responses;
using Domain.Entities;

namespace Application.Features.AMCS
{
    public interface IAmcInstrumentService
    {
        public Task<bool> CreateAmcInstrument(List<AMCInstrument> amcInstrument);
        public Task<bool> DeleteAmcInstrument(Guid id);
        public Task<bool> ExistsInstrumentInAMCQuery(AmcRequest amc);
        Task<AMCInstrument> GetByIdAsync(Guid requestId);
        Task<Guid> UpdateInstrumentAsync(AMCInstrument amcInstrument);
        Task<List<AmcInstrumentResponse>> GetByAmcIdAsync(Guid requestAmcId);
    }
}
