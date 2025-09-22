using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;

namespace Application.Features.Dashboards.Queries
{
    public class GetDistDashboardDataQuery : IRequest<IResponseWrapper>
    {
        public DashboardDateRequest DashboardDateRequest { get; set; }
    }

    public class GetDistDashboardDataQueryHandler(IDistributorDashboardService distributorDashboardService) : IRequestHandler<GetDistDashboardDataQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDistDashboardDataQuery request, CancellationToken cancellationToken)
        {
            var revenueInDb = await distributorDashboardService.GetDistDashboardDataAsync(request.DashboardDateRequest);

            if (revenueInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: revenueInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}