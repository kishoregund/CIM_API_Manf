using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Commands
{
    public class CreateServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ServiceReportRequest ServiceReportRequest { get; set; }
    }

    public class CreateServiceReportCommandHandler(IServiceReportService ServiceReportService) : IRequestHandler<CreateServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateServiceReportCommand request, CancellationToken cancellationToken)
        {
            // map

            var newServiceReport = request.ServiceReportRequest.Adapt<Domain.Entities.ServiceReport>();

            var ServiceReportId = await ServiceReportService.CreateServiceReportAsync(newServiceReport);

            return await ResponseWrapper<Guid>.SuccessAsync(data: ServiceReportId, message: "Record saved successfully.");
        }
    }
}
