using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPConsumedBySRPIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetSPConsumedBySRPIdQueryHandler(ISPConsumedService SPConsumedService) : IRequestHandler<GetSPConsumedBySRPIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPConsumedBySRPIdQuery request, CancellationToken cancellationToken)
        {
            var SPConsumedInDb = await SPConsumedService.GetSPConsumedBySRPIdAsync(request.ServiceReportId);

            if (SPConsumedInDb.Count > 0)
            {
                return await ResponseWrapper<List<SPConsumedResponse>>.SuccessAsync(data: SPConsumedInDb.Adapt<List<SPConsumedResponse>>());
            }
            return await ResponseWrapper<List<SPConsumedResponse>>.SuccessAsync(message: "No SPConsumed were found.");
        }
    }
}
