using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;

namespace Application.Features.Dashboards.Queries
{
    public class GetServiceContractRevenueQuery : IRequest<IResponseWrapper>
    {
        public DashboardDateRequest DashboardDateRequest { get; set; }
    }

    public class GetServiceContractRevenueQueryHandler(IDistributorDashboardService distributorDashboardService) : IRequestHandler<GetServiceContractRevenueQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceContractRevenueQuery request, CancellationToken cancellationToken)
        {
            var revenueInDb = await distributorDashboardService.GetServiceContractRevenue(request.DashboardDateRequest);

            if (revenueInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: revenueInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}