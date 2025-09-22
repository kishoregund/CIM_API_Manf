using Application.Features.ServiceReports.Responses;
using Domain.Views;

namespace Application.Features.ServiceReports.Queries
{
    public class GetServiceReportPDFQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetServiceReportPDFQueryHandler(IServiceReportService ServiceReportService) : IRequestHandler<GetServiceReportPDFQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceReportPDFQuery request, CancellationToken cancellationToken)
        {
            var ServiceReportInDb = await ServiceReportService.GetServiceReportPDFAsync(request.ServiceReportId);

            if (ServiceReportInDb is not null)
            {
                return await ResponseWrapper<VW_ServiceReport>.SuccessAsync(data: ServiceReportInDb);
            }
            return await ResponseWrapper<VW_ServiceReport>.SuccessAsync(message: "Service Report does not exists.");
        }
    }
}
