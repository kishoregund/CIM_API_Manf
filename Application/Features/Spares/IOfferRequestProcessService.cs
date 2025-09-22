using Application.Features.Spares.Responses;
using Domain.Views;

namespace Application.Features.Spares
{
    public interface IOfferRequestProcessService
    {
        Task<OfferRequestProcessResponse> GetOfferRequestProcessAsync(Guid id);
        Task<OfferRequestProcess> GetOfferRequestProcessEntityAsync(Guid id);        
        Task<List<OfferRequestProcessResponse>> GetOfferRequestProcessesAsync(Guid offerRequestId);
        Task<Guid> CreateOfferRequestProcessAsync(OfferRequestProcess OfferRequestProcess);
        Task<Guid> UpdateOfferRequestProcessAsync(OfferRequestProcess OfferRequestProcess);
        Task<bool> DeleteOfferRequestProcessAsync(Guid id);
    }
}
