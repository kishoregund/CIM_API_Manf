using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetAllServiceRequestQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllServiceRequestQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetAllServiceRequestQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllServiceRequestQuery request, CancellationToken cancellationToken)
        {
            var revenueInDb = await CustomerDashboardService.GetAllServiceRequestAsync();

            if (revenueInDb is not null)
            {
                return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(data: revenueInDb);
            }
            return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}