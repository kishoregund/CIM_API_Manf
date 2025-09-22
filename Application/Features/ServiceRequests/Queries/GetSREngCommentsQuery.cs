using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSREngCommentsQuery : IRequest<IResponseWrapper>
    {
        public Guid SREngCommentsId { get; set; }
    }

    public class GetSREngCommentsQueryHandler(ISREngCommentsService SREngCommentsService) : IRequestHandler<GetSREngCommentsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSREngCommentsQuery request, CancellationToken cancellationToken)
        {
            var SREngCommentsInDb = (await SREngCommentsService.GetSREngCommentAsync(request.SREngCommentsId)).Adapt<SREngCommentsResponse>();

            if (SREngCommentsInDb is not null)
            {
                return await ResponseWrapper<SREngCommentsResponse>.SuccessAsync(data: SREngCommentsInDb);
            }
            return await ResponseWrapper<SREngCommentsResponse>.SuccessAsync(message: "SREngComments does not exists.");
        }
    }
}