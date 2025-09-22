

namespace Application.Features.ServiceReports.Commands
{
    public class DeletePastServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid PastServiceReportId { get; set; }
    }

    public class DeletePastServiceReportCommandHandler(IPastServiceReportService PastServiceReportService) : IRequestHandler<DeletePastServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeletePastServiceReportCommand request, CancellationToken cancellationToken)
        {
            var deletedPastServiceReport = await PastServiceReportService.DeletePastServiceReportAsync(request.PastServiceReportId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedPastServiceReport, message: "PastServiceReport deleted successfully.");
        }
    }
}
