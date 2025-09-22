using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;
using Domain.Views;

namespace Application.Features.Dashboards.Queries
{
    public class GetEngSparesRecommendedQuery : IRequest<IResponseWrapper>
    {
        public string Date { get; set; }
    }

    public class GetSparesRecommendedQueryHandler(IEngineerDashboardService EngineerDashboardService) : IRequestHandler<GetEngSparesRecommendedQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngSparesRecommendedQuery request, CancellationToken cancellationToken)
        {
            var srInDb = await EngineerDashboardService.GetSparesRecommendedAsync(request.Date);

            if (srInDb is not null)
            {
                return await ResponseWrapper<List<VW_SparesRecommended>>.SuccessAsync(data: srInDb);
            }
            return await ResponseWrapper<VW_SparesRecommended>.SuccessAsync(message: "Data does not exists.");
        }
    }
}