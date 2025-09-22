
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetPastServiceReportsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetPastServiceReportsQueryHandler(IPastServiceReportService PastServiceReportService) : IRequestHandler<GetPastServiceReportsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetPastServiceReportsQuery request, CancellationToken cancellationToken)
        {
            var PastServiceReportsInDb = await PastServiceReportService.GetPastServiceReportsAsync();

            if (PastServiceReportsInDb.Count > 0)
            {
                return await ResponseWrapper<List<PastServiceReportResponse>>.SuccessAsync(data: PastServiceReportsInDb);
            }
            return await ResponseWrapper<List<PastServiceReportResponse>>.SuccessAsync(message: "No PastServiceReports were found.");
        }
    }
}
