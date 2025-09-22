using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPRecommendedBySRPIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetSPRecommendedBySRPIdQueryHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<GetSPRecommendedBySRPIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPRecommendedBySRPIdQuery request, CancellationToken cancellationToken)
        {
            var SPRecommendedInDb = await SPRecommendedService.GetSPRecommendedBySRPIdAsync(request.ServiceReportId);

            if (SPRecommendedInDb.Count > 0)
            {
                return await ResponseWrapper<List<SPRecommendedResponse>>.SuccessAsync(data: SPRecommendedInDb.Adapt<List<SPRecommendedResponse>>());
            }
            return await ResponseWrapper<List<SPRecommendedResponse>>.SuccessAsync(message: "No SPRecommended were found.");
        }
    }
}
