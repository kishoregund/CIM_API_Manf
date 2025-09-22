using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSREngCommentsBySRIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetSREngCommentsBySRIdQueryHandler(ISREngCommentsService SREngCommentsService) : IRequestHandler<GetSREngCommentsBySRIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSREngCommentsBySRIdQuery request, CancellationToken cancellationToken)
        {
            var SREngCommentsBySRIdInDb = await SREngCommentsService.GetSREngCommentBySRIdAsync(request.ServiceRequestId);

            if (SREngCommentsBySRIdInDb.Count > 0)
            {
                return await ResponseWrapper<List<SREngCommentsResponse>>.SuccessAsync(data: SREngCommentsBySRIdInDb.Adapt<List<SREngCommentsResponse>>());
            }
            return await ResponseWrapper<List<SREngCommentsResponse>>.SuccessAsync(message: "No SREngComments were found.");
        }
    }
}
