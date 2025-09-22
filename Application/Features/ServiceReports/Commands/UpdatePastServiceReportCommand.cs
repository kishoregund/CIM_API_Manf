
using Application.Features.ServiceReports.Requests;
using Domain.Entities;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdatePastServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public PastServiceReportRequest PastServiceReportRequest { get; set; }
    }

    public class UpdatePastServiceReportCommandHandler(IPastServiceReportService PastServiceReportService) : IRequestHandler<UpdatePastServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdatePastServiceReportCommand request, CancellationToken cancellationToken)
        {
            var PastServiceReportInDb = await PastServiceReportService.GetPastServiceReportAsync(request.PastServiceReportRequest.Id);


            PastServiceReportInDb.Id = request.PastServiceReportRequest.Id;
            PastServiceReportInDb.CustomerId = request.PastServiceReportRequest.CustomerId;
            PastServiceReportInDb.InstrumentId = request.PastServiceReportRequest.InstrumentId;
            PastServiceReportInDb.BrandId = request.PastServiceReportRequest.BrandId;
            PastServiceReportInDb.Of = request.PastServiceReportRequest.Of;
            PastServiceReportInDb.SiteId = request.PastServiceReportRequest.SiteId;
            PastServiceReportInDb.UpdatedBy = request.PastServiceReportRequest.UpdatedBy;


            var updatePastServiceReportId = await PastServiceReportService.UpdatePastServiceReportAsync(PastServiceReportInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updatePastServiceReportId, message: "Record updated successfully.");
        }
    }
}
