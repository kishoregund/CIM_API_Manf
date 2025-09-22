using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetEngSparesConsumedQuery : IRequest<IResponseWrapper>
    {
        public string Date{ get; set; }
    }

    public class GetEngSparesConsumedQueryHandler(IEngineerDashboardService EngineerDashboardService) : IRequestHandler<GetEngSparesConsumedQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngSparesConsumedQuery request, CancellationToken cancellationToken)
        {
            var srInDb = await EngineerDashboardService.GetSparesConsumedAsync(request.Date);

            if (srInDb is not null)
            {
                return await ResponseWrapper<List<SparesConsumedResponse>>.SuccessAsync(data: srInDb);
            }
            return await ResponseWrapper<SparesConsumedResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}