using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetOfferRequestsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetOfferRequestsQueryHandler(IOfferRequestService OfferRequestService) : IRequestHandler<GetOfferRequestsQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetOfferRequestsQuery request, CancellationToken cancellationToken)
        {
            var OfferRequestInDb = (await OfferRequestService.GetOfferRequestsAsync());

            if (OfferRequestInDb.Count > 0)
            {
                return await ResponseWrapper<List<OfferRequestResponse>>.SuccessAsync(data: OfferRequestInDb.Adapt<List<OfferRequestResponse>>());
            }

            return await ResponseWrapper<List<OfferRequestResponse>>.SuccessAsync(message: "No OfferRequests were found.");
        }
    }
}