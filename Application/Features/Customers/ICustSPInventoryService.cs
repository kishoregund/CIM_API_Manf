
using Application.Features.Customers.Responses;
using Domain.Views;

namespace Application.Features.Customers
{
    public interface ICustSPInventoryService
    {
        Task<CustSPInventoryResponse> GetCustSPInventoryAsync(Guid id);
        Task<CustSPInventory> GetCustSPInventoryEntityAsync(Guid id);        
        Task<List<VW_SparepartConsumedHistory>> GetSparepartConsumedHistoryAsync(Guid id);
        //Task<List<CustSPInventory>> GetCustSPInventorysAsync(Guid customerId);
        Task<List<CustSPInventoryResponse>> GetCustSPInventorysAsync(Guid contactId, Guid customerId);
        Task<List<CustSPInventoryResponse>> GetCustSPInventoryForServiceReportAsync(Guid serviceReportId);
        Task<Guid> CreateCustSPInventoryAsync(CustSPInventory CustSPInventory);
        Task<Guid> UpdateCustSPInventoryAsync(CustSPInventory CustSPInventory);
        Task<bool> DeleteCustSPInventoryAsync(Guid id);
    }
}
