using Application.Features.ServiceReports.Requests;
using Domain.Entities;
using System;

namespace Application.Features.ServiceReports.Commands
{
    public class UploadServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UploadServiceReportRequest uploadRequest { get; set; }
    }

    public class UploadServiceReportCommandHandler(IServiceReportService serviceReportService) : IRequestHandler<UploadServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UploadServiceReportCommand request, CancellationToken cancellationToken)
        {
            var uploadServiceReport = await serviceReportService.UploadServiceReportAsync(request.uploadRequest);

            return await ResponseWrapper<bool>.SuccessAsync(data: uploadServiceReport, message: "Service Report updated successfully.");
        }
    }
}
