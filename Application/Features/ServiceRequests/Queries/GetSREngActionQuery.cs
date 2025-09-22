using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSREngActionQuery : IRequest<IResponseWrapper>
    {
        public Guid SREngActionId { get; set; }
    }

    public class GetSREngActionQueryHandler(ISREngActionService SREngActionService) : IRequestHandler<GetSREngActionQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSREngActionQuery request, CancellationToken cancellationToken)
        {
            var SREngActionInDb = (await SREngActionService.GetSREngActionAsync(request.SREngActionId)).Adapt<SREngActionResponse>();

            if (SREngActionInDb is not null)
            {
                return await ResponseWrapper<SREngActionResponse>.SuccessAsync(data: SREngActionInDb);
            }
            return await ResponseWrapper<SREngActionResponse>.SuccessAsync(message: "SREngAction does not exists.");
        }
    }
}