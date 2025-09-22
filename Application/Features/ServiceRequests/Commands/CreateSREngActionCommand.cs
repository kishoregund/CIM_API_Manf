
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateSREngActionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SREngActionRequest SREngActionRequest { get; set; }
    }

    public class CreateSREngActionCommandHandler(ISREngActionService SREngActionService) : IRequestHandler<CreateSREngActionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSREngActionCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSREngAction = request.SREngActionRequest.Adapt<SREngAction>();

            var SREngActionId = await SREngActionService.CreateSREngActionAsync(newSREngAction);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SREngActionId, message: "Record saved successfully.");
        }
    }
}
