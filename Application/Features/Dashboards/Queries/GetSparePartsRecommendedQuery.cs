using Application.Features.Dashboards;
using Domain.Views;

namespace Application.Features.Dashboards.Queries
{
    public class GetSparePartsRecommendedQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetSparePartsRecommendedQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetSparePartsRecommendedQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSparePartsRecommendedQuery request, CancellationToken cancellationToken)
        {
            var sparesInDb = await CustomerDashboardService.GetSparePartsRecommendedAsync();

            if (sparesInDb is not null)
            {
                return await ResponseWrapper<List<VW_SparesRecommended>>.SuccessAsync(data: sparesInDb);
            }
            return await ResponseWrapper<VW_SparesRecommended>.SuccessAsync(message: "Data does not exists.");
        }
    }
}