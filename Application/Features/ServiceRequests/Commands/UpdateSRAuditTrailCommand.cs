

using Application.Features.ServiceRequests.Requests;
using Domain.Entities;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateSRAuditTrailCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRAuditTrailRequest SRAuditTrailRequest { get; set; }
    }

    public class UpdateSRAuditTrailCommandHandler(ISRAuditTrailService SRAuditTrailService) : IRequestHandler<UpdateSRAuditTrailCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSRAuditTrailCommand request, CancellationToken cancellationToken)
        {
            var SRAuditTrailInDb = await SRAuditTrailService.GetSRAuditTrailAsync(request.SRAuditTrailRequest.Id);

            SRAuditTrailInDb.Id = request.SRAuditTrailRequest.Id;
            SRAuditTrailInDb.ServiceRequestId = request.SRAuditTrailRequest.ServiceRequestId;
            SRAuditTrailInDb.Action = request.SRAuditTrailRequest.Action;
            SRAuditTrailInDb.UserId = request.SRAuditTrailRequest.UserId;
            SRAuditTrailInDb.Values = request.SRAuditTrailRequest.Values;
            SRAuditTrailInDb.UpdatedBy = request.SRAuditTrailRequest.UpdatedBy;


            var updateSRAuditTrailId = await SRAuditTrailService.UpdateSRAuditTrailAsync(SRAuditTrailInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSRAuditTrailId, message: "Record updated successfully.");
        }
    }
}
