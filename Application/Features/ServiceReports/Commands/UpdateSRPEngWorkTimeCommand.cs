using Application.Features.ServiceReports.Requests;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdateSRPEngWorkTimeCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRPEngWorkTimeRequest SRPEngWorkTimeRequest { get; set; }
    }

    public class UpdateSRPEngWorkTimeCommandHandler(ISRPEngWorkTimeService SRPEngWorkTimeService) : IRequestHandler<UpdateSRPEngWorkTimeCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSRPEngWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var SRPEngWorkTimeInDb = await SRPEngWorkTimeService.GetSRPEngWorkTimeAsync(request.SRPEngWorkTimeRequest.Id);


            SRPEngWorkTimeInDb.Id = request.SRPEngWorkTimeRequest.Id;
            SRPEngWorkTimeInDb.EndTime = request.SRPEngWorkTimeRequest.EndTime;
            SRPEngWorkTimeInDb.PerDayHrs = request.SRPEngWorkTimeRequest.PerDayHrs;
            SRPEngWorkTimeInDb.StartTime = request.SRPEngWorkTimeRequest.StartTime;
            SRPEngWorkTimeInDb.TotalDays = request.SRPEngWorkTimeRequest.TotalDays;
            SRPEngWorkTimeInDb.TotalHrs = request.SRPEngWorkTimeRequest.TotalHrs;
            SRPEngWorkTimeInDb.WorkTimeDate = request.SRPEngWorkTimeRequest.WorkTimeDate;
            SRPEngWorkTimeInDb.ServiceReportId = request.SRPEngWorkTimeRequest.ServiceReportId;
            SRPEngWorkTimeInDb.UpdatedBy = request.SRPEngWorkTimeRequest.UpdatedBy;


            var updateSRPEngWorkTimeId = await SRPEngWorkTimeService.UpdateSRPEngWorkTimeAsync(SRPEngWorkTimeInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSRPEngWorkTimeId, message: "Record updated successfully.");
        }
    }
}
