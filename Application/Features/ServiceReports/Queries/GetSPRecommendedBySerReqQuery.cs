using Application.Features.ServiceReports.Responses;
using Domain.Views;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPRecommendedBySerReqQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetSPRecommendedBySerReqQueryHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<GetSPRecommendedBySerReqQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPRecommendedBySerReqQuery request, CancellationToken cancellationToken)
        {
            var SPRecommendedInDb = await SPRecommendedService.GetSPRecommendedBySerReqAsync(request.ServiceRequestId);

            if (SPRecommendedInDb.Count > 0)
            {
                return await ResponseWrapper<List<VW_Spareparts>>.SuccessAsync(data: SPRecommendedInDb);
            }
            return await ResponseWrapper<List<VW_Spareparts>>.SuccessAsync(message: "No Spareparts were found.");
        }
    }
}
