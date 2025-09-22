using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetServiceReportsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetServiceReportsQueryHandler(IServiceReportService ServiceReportService) : IRequestHandler<GetServiceReportsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceReportsQuery request, CancellationToken cancellationToken)
        {
            var ServiceReportsInDb = await ServiceReportService.GetServiceReportsAsync();

            if (ServiceReportsInDb.Count > 0)
            {
                return await ResponseWrapper<List<ServiceReportResponse>>.SuccessAsync(data: ServiceReportsInDb.Adapt<List<ServiceReportResponse>>());
            }
            return await ResponseWrapper<List<ServiceReportResponse>>.SuccessAsync(message: "No ServiceReports were found.");
        }
    }
}
