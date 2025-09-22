using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards
{
    public interface IDistributorDashboardService
    {
        Task<List<ServiceRequestRaisedResponse>> GetServiceRequestRaised(string businessUnitId, string brandId);
        Task<List<ServiceRequestRaisedResponse>> GetInstrumentsInstalled(string businessUnitId, string brandId);
        Task<List<ServiceRequestRaisedResponse>> GetEngHandlingServiceRequest(string businessUnitId, string brandId);
        Task<List<ServiceRequestRaisedResponse>> GetInstByHighestServiceRequest(string businessUnitId, string brandId);
        Task<object> GetInstrumentInstalled(DashboardDateRequest dashboardDate);
        Task<ServiceContractRevenueResponse> GetServiceContractRevenue(DashboardDateRequest dashboardDate);
        Task<object> GetRevenueFromCustomer(DashboardDateRequest dashboardDate);
        Task<DistDashboardSerReqModel> GetDistDashboardDataAsync(DashboardDateRequest dashboardDate);
    }
}
