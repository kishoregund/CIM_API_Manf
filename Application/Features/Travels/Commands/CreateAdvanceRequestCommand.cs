using Application.Features.AppBasic.Requests;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Application.Features.Travels;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class CreateAdvanceRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public AdvanceRequest Request { get; set; }
    }
    public class CreateAdvanceRequestCommandHandler(IAdvanceRequestService AdvanceRequestService)
        : IRequestHandler<CreateAdvanceRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateAdvanceRequestCommand request, CancellationToken cancellationToken)
        {
            var AdvanceRequest = request.Request.Adapt<AdvanceRequest>();
            var result = await AdvanceRequestService.CreateAdvanceRequestAsync(AdvanceRequest);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record saved successfully.");
        }
    }
}