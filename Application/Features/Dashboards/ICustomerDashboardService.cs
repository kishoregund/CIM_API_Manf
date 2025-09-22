using Application.Features.AMCS.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;
using Application.Features.ServiceRequests.Responses;
using Application.Features.Spares.Responses;
using Application.Features.Tenancy.Models;
using Domain.Views;

namespace Application.Features.Dashboards
{
    public interface ICustomerDashboardService
    {
        Task<InstrumentOwnershipResponse> GetCostOfOwnerShipAsync(string id);
        Task<object> GetCostDataAsync(DashboardDateRequest dashboardDateModel);
        Task<List<ServiceRequestResponse>> GetAllServiceRequestAsync();
        Task<List<AmcResponse>> GetAllAmcAsync();
        Task<List<VW_SparesRecommended>> GetSparePartsRecommendedAsync();
        Task<CustomerResponse> GetCustomerDetailsAsync();
        Task<List<OfferRequestResponse>> GetAllOfferrequestAsync();
        Task<List<CustomerInstrumentResponse>> GetSiteInstrumentAsync(string siteId);
        Task<List<CustomerInstrumentResponse>> GetSerReqInstrumentAsync(string instrId);
    }
}
