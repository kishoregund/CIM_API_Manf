using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetServiceReportQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetServiceReportQueryHandler(IServiceReportService ServiceReportService) : IRequestHandler<GetServiceReportQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceReportQuery request, CancellationToken cancellationToken)
        {
            var ServiceReportInDb = (await ServiceReportService.GetServiceReportAsync(request.ServiceReportId)).Adapt<ServiceReportResponse>();

            if (ServiceReportInDb is not null)
            {
                return await ResponseWrapper<ServiceReportResponse>.SuccessAsync(data: ServiceReportInDb);
            }
            return await ResponseWrapper<ServiceReportResponse>.SuccessAsync(message: "ServiceReport does not exists.");
        }
    }
}
