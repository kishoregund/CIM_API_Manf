using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSRPEngWorkTimeBySRPIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetSRPEngWorkTimeBySRPIdQueryHandler(ISRPEngWorkTimeService SRPEngWorkTimeService) : IRequestHandler<GetSRPEngWorkTimeBySRPIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRPEngWorkTimeBySRPIdQuery request, CancellationToken cancellationToken)
        {
            var SRPEngWorkTimeInDb = await SRPEngWorkTimeService.GetSRPEngWorkTimeBySRPIdAsync(request.ServiceReportId);

            if (SRPEngWorkTimeInDb.Count > 0)
            {
                return await ResponseWrapper<List<SRPEngWorkTimeResponse>>.SuccessAsync(data: SRPEngWorkTimeInDb.Adapt<List<SRPEngWorkTimeResponse>>());
            }
            return await ResponseWrapper<List<SRPEngWorkTimeResponse>>.SuccessAsync(message: "No SRPEngWorkTime were found.");
        }
    }
}
