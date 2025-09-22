using Application.Features.ServiceReports.Requests;
using Domain.Entities;

namespace Application.Features.ServiceReports.Commands
{
    public class UpdateSRPEngWorkDoneCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRPEngWorkDoneRequest SRPEngWorkDoneRequest { get; set; }
    }

    public class UpdateSRPEngWorkDoneCommandHandler(ISRPEngWorkDoneService SRPEngWorkDoneService) : IRequestHandler<UpdateSRPEngWorkDoneCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSRPEngWorkDoneCommand request, CancellationToken cancellationToken)
        {
            var SRPEngWorkDoneInDb = await SRPEngWorkDoneService.GetSRPEngWorkDoneAsync(request.SRPEngWorkDoneRequest.Id);


            SRPEngWorkDoneInDb.Id = request.SRPEngWorkDoneRequest.Id;
            SRPEngWorkDoneInDb.Remarks = request.SRPEngWorkDoneRequest.Remarks;
            SRPEngWorkDoneInDb.Workdone = request.SRPEngWorkDoneRequest.Workdone;                
            SRPEngWorkDoneInDb.ServiceReportId = request.SRPEngWorkDoneRequest.ServiceReportId;
            SRPEngWorkDoneInDb.UpdatedBy = request.SRPEngWorkDoneRequest.UpdatedBy;


            var updateSRPEngWorkDoneId = await SRPEngWorkDoneService.UpdateSRPEngWorkDoneAsync(SRPEngWorkDoneInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSRPEngWorkDoneId, message: "Record updated successfully.");
        }
    }
}
