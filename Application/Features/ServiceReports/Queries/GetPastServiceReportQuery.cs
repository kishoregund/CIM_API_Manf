
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetPastServiceReportQuery : IRequest<IResponseWrapper>
    {
        public Guid PastServiceReportId { get; set; }
    }

    public class GetPastServiceReportQueryHandler(IPastServiceReportService PastServiceReportService) : IRequestHandler<GetPastServiceReportQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetPastServiceReportQuery request, CancellationToken cancellationToken)
        {
            var pastServiceReportInDb = (await PastServiceReportService.GetPastServiceReportAsync(request.PastServiceReportId)).Adapt<PastServiceReportResponse>();

            if (pastServiceReportInDb is not null)
            {
                return await ResponseWrapper<PastServiceReportResponse>.SuccessAsync(data: pastServiceReportInDb);
            }
            return await ResponseWrapper<PastServiceReportResponse>.SuccessAsync(message: "PastServiceReport does not exists.");
        }
    }
}
