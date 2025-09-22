using Application.Features.Spares.Responses;
using Domain.Views;

namespace Application.Features.Spares
{
    public interface IOfferRequestService
    {
        Task<OfferRequestResponse> GetOfferRequestAsync(Guid id);
        Task<OfferRequest> GetOfferRequestEntityAsync(Guid id);        
        Task<List<OfferRequestResponse>> GetOfferRequestsAsync();
        Task<List<SparepartsOfferRequestResponse>> GetSparepartsByInstrumentPartNoAsync(string instrumentIds, string partNo);
        Task<Guid> CreateOfferRequestAsync(OfferRequest OfferRequest);
        Task<Guid> UpdateOfferRequestAsync(OfferRequest OfferRequest);
        Task<bool> DeleteOfferRequestAsync(Guid id);
    }
}
