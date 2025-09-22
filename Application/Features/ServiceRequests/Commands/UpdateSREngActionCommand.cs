using Application.Features.ServiceRequests.Requests;
using Domain.Entities;
using System.Xml.Linq;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateSREngActionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SREngActionRequest SREngActionRequest { get; set; }
    }

    public class UpdateSREngActionCommandHandler(ISREngActionService SREngActionService) : IRequestHandler<UpdateSREngActionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSREngActionCommand request, CancellationToken cancellationToken)
        {
            var SREngActionInDb = await SREngActionService.GetSREngActionAsync(request.SREngActionRequest.Id);

            SREngActionInDb.Id = request.SREngActionRequest.Id;
            SREngActionInDb.ServiceRequestId = request.SREngActionRequest.ServiceRequestId;
            SREngActionInDb.EngineerId = request.SREngActionRequest.EngineerId;
            SREngActionInDb.ActionDate = request.SREngActionRequest.ActionDate;
            SREngActionInDb.Actiontaken = request.SREngActionRequest.Actiontaken;
            SREngActionInDb.Comments = request.SREngActionRequest.Comments;
            SREngActionInDb.TeamviewRecording = request.SREngActionRequest.TeamviewRecording;
            SREngActionInDb.UpdatedBy = request.SREngActionRequest.UpdatedBy;

            var updateSREngActionId = await SREngActionService.UpdateSREngActionAsync(SREngActionInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSREngActionId, message: "Record updated successfully.");
        }
    }
}
