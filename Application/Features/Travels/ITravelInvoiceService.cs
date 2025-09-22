using Application.Features.AppBasic.Responses;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels
{
    public interface ITravelInvoiceService
    {
        Task<TravelInvoice> GetTravelInvoiceEntityAsync(Guid id);
        Task<TravelInvoiceResponse> GetTravelInvoiceByIdAsync(Guid id);
        Task<List<TravelInvoiceResponse>> GetTravelInvoicesAsync(string businessUnitId, string brandId);
        Task<Guid> CreateTravelInvoiceAsync(TravelInvoice travelInvoice);
        Task<Guid> UpdateTravelInvoiceAsync(TravelInvoice travelInvoice);
        Task<bool> DeleteTravelInvoiceAsync(Guid id);
    }
}
