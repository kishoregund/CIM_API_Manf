using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartsOfferRequestsQuery : IRequest<IResponseWrapper>
    {
        public Guid OfferRequestId { get; set; }
    }

    public class GetSparepartsOfferRequestsQueryHandler(ISparepartsOfferRequestService SparepartsOfferRequestervice) : IRequestHandler<GetSparepartsOfferRequestsQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetSparepartsOfferRequestsQuery request, CancellationToken cancellationToken)
        {
            var OfferRequestInDb = (await SparepartsOfferRequestervice.GetSparepartsOfferRequestsAsync(request.OfferRequestId));

            if (OfferRequestInDb.Count > 0)
            {
                return await ResponseWrapper<List<SparepartsOfferRequestResponse>>.SuccessAsync(data: OfferRequestInDb);
            }

            return await ResponseWrapper<SparepartsOfferRequestResponse>.SuccessAsync(message: "No SparepartsOfferRequest were found.");
        }
    }
}