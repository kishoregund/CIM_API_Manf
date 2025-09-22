using Application.Features.ServiceReports;
using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSPConsumedQuery : IRequest<IResponseWrapper>
    {
        public Guid SPConsumedId { get; set; }
    }

    public class GetSPConsumedQueryHandler(ISPConsumedService SPConsumedService) : IRequestHandler<GetSPConsumedQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSPConsumedQuery request, CancellationToken cancellationToken)
        {
            var SPConsumedInDb = (await SPConsumedService.GetSPConsumedAsync(request.SPConsumedId)).Adapt<SPConsumedResponse>();

            if (SPConsumedInDb is not null)
            {
                return await ResponseWrapper<SPConsumedResponse>.SuccessAsync(data: SPConsumedInDb);
            }
            return await ResponseWrapper<SPConsumedResponse>.SuccessAsync(message: "SPConsumed does not exists.");
        }
    }
}
