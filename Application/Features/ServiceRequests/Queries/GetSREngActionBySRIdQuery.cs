using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSREngActionBySRIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetSREngActionBySRIdQueryHandler(ISREngActionService SREngActionService) : IRequestHandler<GetSREngActionBySRIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSREngActionBySRIdQuery request, CancellationToken cancellationToken)
        {
            var SREngActionBySRIdInDb = await SREngActionService.GetSREngActionBySRIdAsync(request.ServiceRequestId);

            if (SREngActionBySRIdInDb.Count > 0)
            {
                return await ResponseWrapper<List<SREngActionResponse>>.SuccessAsync(data: SREngActionBySRIdInDb.Adapt<List<SREngActionResponse>>());
            }
            return await ResponseWrapper<List<SREngActionResponse>>.SuccessAsync(message: "No SREngAction were found.");
        }
    }
}
