
using Application.Features.ServiceReports.Responses;
using Application.Features.ServiceReports.Requests;

namespace Application.Features.ServiceReports.Commands
{
    public class CreatePastServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public PastServiceReportRequest PastServiceReportRequest { get; set; }
    }

    public class CreatePastServiceReportCommandHandler(IPastServiceReportService PastServiceReportService) : IRequestHandler<CreatePastServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreatePastServiceReportCommand request, CancellationToken cancellationToken)
        {
            // map

            var newPastServiceReport = request.PastServiceReportRequest.Adapt<PastServiceReport>();

            var PastServiceReportId = await PastServiceReportService.CreatePastServiceReportAsync(newPastServiceReport);

            return await ResponseWrapper<Guid>.SuccessAsync(data: PastServiceReportId, message: "Record saved successfully.");
        }
    }
}
