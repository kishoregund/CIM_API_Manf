using Application.Features.AppBasic.Responses;

namespace Application.Features.Travels
{
    public interface ITravelExpenseItemsService
    {        
        Task<List<TravelExpenseItemsResponse>> GetTravelExpenseItemsAsync(Guid expenseId);
        Task<TravelExpenseItems> GetTravelExpenseItemsEntityAsync(Guid id);
        Task<TravelExpenseItemsResponse> GetTravelExpenseItemsByIdAsync(Guid id);
        Task<Guid> CreateTravelExpenseItemsAsync(TravelExpenseItems travelExpenseItems);
        Task<bool> DeleteTravelExpenseItemsAsync(Guid travelExpenseItemId);
        Task<Guid> UpdateTravelExpenseItemsAsync(TravelExpenseItems travelExpenseItems);
    }
}