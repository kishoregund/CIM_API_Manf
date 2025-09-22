using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPRecommendedQuery : IRequest<IResponseWrapper>
    {
        public Guid SPRecommendedId { get; set; }
    }

    public class GetSPRecommendedQueryHandler(ISPRecommendedService SPRecommendedService) : IRequestHandler<GetSPRecommendedQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPRecommendedQuery request, CancellationToken cancellationToken)
        {
            var SPRecommendedInDb = (await SPRecommendedService.GetSPRecommendedAsync(request.SPRecommendedId)).Adapt<SPRecommendedResponse>();

            if (SPRecommendedInDb is not null)
            {
                return await ResponseWrapper<SPRecommendedResponse>.SuccessAsync(data: SPRecommendedInDb);
            }
            return await ResponseWrapper<SPRecommendedResponse>.SuccessAsync(message: "SPRecommended does not exists.");
        }
    }
}
