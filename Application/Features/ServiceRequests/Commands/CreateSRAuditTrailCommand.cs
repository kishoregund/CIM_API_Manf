
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateSRAuditTrailCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRAuditTrailRequest SRAuditTrailRequest { get; set; }
    }

    public class CreateSRAuditTrailCommandHandler(ISRAuditTrailService SRAuditTrailService) : IRequestHandler<CreateSRAuditTrailCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSRAuditTrailCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSRAuditTrail = request.SRAuditTrailRequest.Adapt<SRAuditTrail>();

            var SRAuditTrailId = await SRAuditTrailService.CreateSRAuditTrailAsync(newSRAuditTrail);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SRAuditTrailId, message: "Record saved successfully.");
        }
    }
}
