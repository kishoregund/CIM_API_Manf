using Application.Features.ServiceRequests.Requests;
using System.Xml.Linq;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateSREngCommentsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SREngCommentsRequest SREngCommentsRequest { get; set; }
    }

    public class UpdateSREngCommentsCommandHandler(ISREngCommentsService SREngCommentsService) : IRequestHandler<UpdateSREngCommentsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSREngCommentsCommand request, CancellationToken cancellationToken)
        {
            var SREngCommentsInDb = await SREngCommentsService.GetSREngCommentAsync(request.SREngCommentsRequest.Id);

            SREngCommentsInDb.Id = request.SREngCommentsRequest.Id;
            SREngCommentsInDb.ServiceRequestId = request.SREngCommentsRequest.ServiceRequestId;
            SREngCommentsInDb.EngineerId = request.SREngCommentsRequest.EngineerId;
            SREngCommentsInDb.Comments = request.SREngCommentsRequest.Comments;
            SREngCommentsInDb.Nextdate = request.SREngCommentsRequest.Nextdate;
            SREngCommentsInDb.UpdatedBy = request.SREngCommentsRequest.UpdatedBy;

            var updateSREngCommentsId = await SREngCommentsService.UpdateSREngCommentAsync(SREngCommentsInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSREngCommentsId, message: "Record updated successfully.");
        }
    }
}
