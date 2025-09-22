using Application.Features.AppBasic.Responses;
using Application.Features.Travels;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetAdvanceRequestByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid AdvanceRequestId { get; set; }
    }

    public class GetAdvanceRequestByIdQueryHandler(IAdvanceRequestService AdvanceRequestService) : IRequestHandler<GetAdvanceRequestByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAdvanceRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var AdvanceRequestInDb = (await AdvanceRequestService.GetAdvanceRequestByIdAsync(request.AdvanceRequestId)).Adapt<AdvanceRequestResponse>();

            if (AdvanceRequestInDb is not null)
            {
                return await ResponseWrapper<AdvanceRequestResponse>.SuccessAsync(data: AdvanceRequestInDb);
            }
            return await ResponseWrapper<AdvanceRequestResponse>.SuccessAsync(message: "AdvanceRequest does not exists.");
        }
    }
}