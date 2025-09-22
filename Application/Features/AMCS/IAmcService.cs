using Application.Features.AMCS.Responses;
using Application.Features.Dashboards.Responses;
using Domain.Entities;

namespace Application.Features.AMCS
{
    public interface IAmcService
    {
        public Task<Guid> CreateAmc(AMC amc);
        public Task<bool> DeleteAmc (Guid id);
        public Task<AMCResponse> GetByIdAsync(Guid requestId);
        public Task<AMC> GetByIdEntityAsync(Guid requestId);
        public Task<List<AmcResponse>> GetAllAsync();
        public Task<Guid> UpdateAmcAsync(AMC updateAmc);
        public Task<bool> ServiceQuoteExists(string serviceQuote);
        
    }
}
