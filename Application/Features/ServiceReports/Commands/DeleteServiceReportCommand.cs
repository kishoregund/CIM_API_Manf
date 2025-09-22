namespace Application.Features.ServiceReports.Commands
{
    public class DeleteServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid ServiceReportId { get; set; }
    }

    public class DeleteServiceReportCommandHandler(IServiceReportService ServiceReportService) : IRequestHandler<DeleteServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteServiceReportCommand request, CancellationToken cancellationToken)
        {
            var deletedServiceReport = await ServiceReportService.DeleteServiceReportAsync(request.ServiceReportId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedServiceReport, message: "ServiceReport deleted successfully.");
        }
    }
}
