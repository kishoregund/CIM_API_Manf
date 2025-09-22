using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetEngServiceRequestQuery : IRequest<IResponseWrapper>
    {
        public string Date{ get; set; }
    }

    public class GetEngServiceRequestQueryHandler(IEngineerDashboardService EngineerDashboardService) : IRequestHandler<GetEngServiceRequestQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngServiceRequestQuery request, CancellationToken cancellationToken)
        {
            var srInDb = await EngineerDashboardService.GetServiceRequestAsync(request.Date);

            if (srInDb is not null)
            {
                return await ResponseWrapper<List<EngServiceRequestResponse>>.SuccessAsync(data: srInDb);
            }
            return await ResponseWrapper<EngServiceRequestResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}