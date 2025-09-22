using Application.Features.ServiceReports.Requests;
using Domain.Entities;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdateServiceReportCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ServiceReportRequest ServiceReportRequest { get; set; }
    }

    public class UpdateServiceReportCommandHandler(IServiceReportService ServiceReportService) : IRequestHandler<UpdateServiceReportCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateServiceReportCommand request, CancellationToken cancellationToken)
        {
            var ServiceReportInDb = await ServiceReportService.GetServiceReportEntityAsync(request.ServiceReportRequest.Id);


            ServiceReportInDb.Id = request.ServiceReportRequest.Id;
            ServiceReportInDb.AnalyticalAssit = request.ServiceReportRequest.AnalyticalAssit;
            //ServiceReportInDb.BrandId = request.ServiceReportRequest.BrandId;
            ServiceReportInDb.ComputerArlsn = request.ServiceReportRequest.ComputerArlsn;
            ServiceReportInDb.CorrMaintenance = request.ServiceReportRequest.CorrMaintenance;
            ServiceReportInDb.Country = request.ServiceReportRequest.Country;
            ServiceReportInDb.Customer = request.ServiceReportRequest.Customer;
            ServiceReportInDb.CustSignature = request.ServiceReportRequest.CustSignature;
            ServiceReportInDb.Department = request.ServiceReportRequest.Department;
            ServiceReportInDb.EngineerComments = request.ServiceReportRequest.EngineerComments;
            ServiceReportInDb.EngSignature = request.ServiceReportRequest.EngSignature;
            ServiceReportInDb.Firmaware = request.ServiceReportRequest.Firmaware;
            ServiceReportInDb.Installation = request.ServiceReportRequest.Installation;
            //ServiceReportInDb.InstrumentId = request.ServiceReportRequest.InstrumentId;
            ServiceReportInDb.IsCompleted = request.ServiceReportRequest.IsCompleted;
            ServiceReportInDb.Interrupted = request.ServiceReportRequest.Interrupted;
            ServiceReportInDb.LabChief = request.ServiceReportRequest.LabChief;
            ServiceReportInDb.NextVisitScheduled = request.ServiceReportRequest.NextVisitScheduled;
            ServiceReportInDb.PrevMaintenance = request.ServiceReportRequest.PrevMaintenance;
            ServiceReportInDb.Problem = request.ServiceReportRequest.Problem;
            ServiceReportInDb.Reason = request.ServiceReportRequest.Reason;
            ServiceReportInDb.RespInstrumentId = request.ServiceReportRequest.RespInstrumentId;
            ServiceReportInDb.Rework = request.ServiceReportRequest.Rework;
            ServiceReportInDb.ServiceReportDate = request.ServiceReportRequest.ServiceReportDate;
            ServiceReportInDb.ServiceReportNo = request.ServiceReportRequest.ServiceReportNo;
            //ServiceReportInDb.ServiceRequestId = request.ServiceReportRequest.ServiceRequestId;
            ServiceReportInDb.SignCustName = request.ServiceReportRequest.SignCustName;
            ServiceReportInDb.SignEngName = request.ServiceReportRequest.SignEngName;
            ServiceReportInDb.Software = request.ServiceReportRequest.Software;
            ServiceReportInDb.SrOf = request.ServiceReportRequest.SrOf;
            ServiceReportInDb.Town = request.ServiceReportRequest.Town;
            ServiceReportInDb.WorkCompleted = request.ServiceReportRequest.WorkCompleted;
            //ServiceReportInDb.WorkDone = request.ServiceReportRequest.WorkDone;
            ServiceReportInDb.WorkFinished = request.ServiceReportRequest.WorkFinished;
            //ServiceReportInDb.WorkTime = request.ServiceReportRequest.WorkTime;


            var updateServiceReportId = await ServiceReportService.UpdateServiceReportAsync(ServiceReportInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateServiceReportId, message: "Record updated successfully.");
        }
    }
}
