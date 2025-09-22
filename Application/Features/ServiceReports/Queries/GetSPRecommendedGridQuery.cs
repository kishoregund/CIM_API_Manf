using Application.Features.ServiceReports.Responses;
using Application.Models;
using Domain.Views;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPRecommendedGridQuery : IRequest<IResponseWrapper>
    {
        public BUBrand BUBrand { get; set; }
    }

    public class GetSPRecommendedGridQueryHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<GetSPRecommendedGridQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPRecommendedGridQuery request, CancellationToken cancellationToken)
        {
            var SPRecommendedInDb = await SPRecommendedService.GetSPRecommendedGridAsync(request.BUBrand.BusinessUnitId, request.BUBrand.BrandId);

            if (SPRecommendedInDb is not null)
            {
                return await ResponseWrapper<List<VW_SparesRecommended>>.SuccessAsync(data: SPRecommendedInDb);
            }
            return await ResponseWrapper<VW_SparesRecommended>.SuccessAsync(message: "Spares Recommended does not exists.");
        }
    }
}
