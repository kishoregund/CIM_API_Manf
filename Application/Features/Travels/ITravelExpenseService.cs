using Application.Features.AppBasic.Responses;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels
{
    public interface ITravelExpenseService
    {
        Task<TravelExpense> GetTravelExpenseEntityByIdAsync(Guid id);
        Task<TravelExpenseResponse> GetTravelExpenseByIdAsync(Guid id);
        Task<List<TravelExpenseResponse>> GetTravelExpensesAsync(string businessUnitId, string brandId);
        Task<Guid> CreateTravelExpenseAsync(TravelExpense TravelExpense);
        Task<Guid> UpdateTravelExpenseAsync(TravelExpense TravelExpense);
        Task<bool> DeleteTravelExpenseAsync(Guid id);
    }
}
