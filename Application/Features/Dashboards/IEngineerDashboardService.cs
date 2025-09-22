using Application.Features.Dashboards.Responses;
using Domain.Views;

namespace Application.Features.Dashboards
{
    public interface IEngineerDashboardService
    {
        Task<List<EngServiceRequestResponse>> GetServiceRequestAsync(string date);
        Task<List<VW_SparesRecommended>> GetSparesRecommendedAsync(string date);
        Task<List<SparesConsumedResponse>> GetSparesConsumedAsync(string date);
        Task<object> GetTravelExpensesAsync(string date);
    }
}