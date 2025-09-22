using Application.Features.AMCS.Responses;
using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetAllAmcQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllAmcQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetAllAmcQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllAmcQuery request, CancellationToken cancellationToken)
        {
            var amcInDb = await CustomerDashboardService.GetAllAmcAsync();

            if (amcInDb is not null)
            {
                return await ResponseWrapper<List<AmcResponse>>.SuccessAsync(data: amcInDb);
            }
            return await ResponseWrapper<AmcResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}