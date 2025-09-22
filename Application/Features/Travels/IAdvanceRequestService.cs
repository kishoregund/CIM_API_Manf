using Application.Features.AppBasic.Responses;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels
{
    public interface IAdvanceRequestService
    {
        Task<AdvanceRequest> GetAdvanceRequestEntityByIdAsync(Guid id);
        Task<AdvanceRequestResponse> GetAdvanceRequestByIdAsync(Guid id);
        Task<List<AdvanceRequestResponse>> GetAdvanceRequestsAsync(string businessUnitId, string brandId);
        Task<Guid> CreateAdvanceRequestAsync(AdvanceRequest AdvanceRequest);
        Task<Guid> UpdateAdvanceRequestAsync(AdvanceRequest AdvanceRequest);
        Task<bool> DeleteAdvanceRequestAsync(Guid id);
    }
}
