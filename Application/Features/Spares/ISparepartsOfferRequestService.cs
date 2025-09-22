using Application.Features.Spares.Requests;
using Application.Features.Spares.Responses;
using Domain.Views;

namespace Application.Features.Spares
{
    public interface ISparepartsOfferRequestService
    {
        Task<SparepartsOfferRequestResponse> GetSparepartsOfferRequestAsync(Guid id);
        Task<SparepartsOfferRequest> GetSparepartsOfferRequestEntityAsync(Guid id);        
        Task<List<SparepartsOfferRequestResponse>> GetSparepartsOfferRequestsAsync(Guid offerRequestId);
        Task<bool> CreateSparepartsOfferRequestAsync(List<SparepartOfferRequestRequest> SparepartsOfferRequest);
        Task<Guid> UpdateSparepartsOfferRequestAsync(SparepartsOfferRequest SparepartsOfferRequest);
        Task<bool> DeleteSparepartsOfferRequestAsync(Guid id);
    }
}
