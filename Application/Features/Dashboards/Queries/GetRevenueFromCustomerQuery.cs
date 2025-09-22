using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;

namespace Application.Features.Dashboards.Queries
{
    public class GetRevenueFromCustomerQuery : IRequest<IResponseWrapper>
    {
        public DashboardDateRequest DashboardDateRequest { get; set; }
    }

    public class GetRevenueFromCustomerQueryHandler(IDistributorDashboardService distributorDashboardService) : IRequestHandler<GetRevenueFromCustomerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRevenueFromCustomerQuery request, CancellationToken cancellationToken)
        {
            var revenueInDb = await distributorDashboardService.GetRevenueFromCustomer(request.DashboardDateRequest);

            if (revenueInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: revenueInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}