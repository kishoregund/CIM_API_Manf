using Application.Features.SRAssignedHistorys.Commands;
using Application.Features.SRAssignedHistorys;
using Domain.Entities;
using System.Xml.Linq;
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateSRAssignedHistoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRAssignedHistoryRequest SRAssignedHistoryRequest { get; set; }
    }

    public class UpdateSRAssignedHistoryCommandHandler(ISRAssignedHistoryService SRAssignedHistoryService) : IRequestHandler<UpdateSRAssignedHistoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSRAssignedHistoryCommand request, CancellationToken cancellationToken)
        {
            var SRAssignedHistoryInDb = await SRAssignedHistoryService.GetSRAssignedHistoryAsync(request.SRAssignedHistoryRequest.Id);

            SRAssignedHistoryInDb.Id = request.SRAssignedHistoryRequest.Id;
            SRAssignedHistoryInDb.AssignedDate = request.SRAssignedHistoryRequest.AssignedDate;
            SRAssignedHistoryInDb.Comments = request.SRAssignedHistoryRequest.Comments;
            SRAssignedHistoryInDb.EngineerId = request.SRAssignedHistoryRequest.EngineerId;
            SRAssignedHistoryInDb.ServiceRequestId = request.SRAssignedHistoryRequest.ServiceRequestId;
            SRAssignedHistoryInDb.TicketStatus = request.SRAssignedHistoryRequest.TicketStatus;
            SRAssignedHistoryInDb.UpdatedBy = request.SRAssignedHistoryRequest.UpdatedBy;

            var updateSRAssignedHistoryId = await SRAssignedHistoryService.UpdateSRAssignedHistoryAsync(SRAssignedHistoryInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSRAssignedHistoryId, message: "Record updated successfully.");
        }
    }
}
