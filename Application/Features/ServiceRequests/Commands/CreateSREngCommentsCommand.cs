using Application.Features.ServiceRequests.Requests;
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateSREngCommentsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SREngCommentsRequest SREngCommentsRequest { get; set; }
    }

    public class CreateSREngCommentsCommandHandler(ISREngCommentsService SREngCommentsService) : IRequestHandler<CreateSREngCommentsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSREngCommentsCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSREngComments = request.SREngCommentsRequest.Adapt<SREngComments>();

            var SREngCommentsId = await SREngCommentsService.CreateSREngCommentAsync(newSREngComments);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SREngCommentsId, message: "Record saved successfully.");
        }
    }
}
