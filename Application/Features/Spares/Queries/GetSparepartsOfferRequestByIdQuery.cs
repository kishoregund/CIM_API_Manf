using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartsOfferRequestByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetSparepartsOfferRequestByIdQueryHandler(ISparepartsOfferRequestService SparepartsOfferRequestService)
        : IRequestHandler<GetSparepartsOfferRequestByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSparepartsOfferRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var SparepartsOfferRequestInDb = (await SparepartsOfferRequestService.GetSparepartsOfferRequestAsync(request.Id)).Adapt<SparepartsOfferRequestResponse>();

            if (SparepartsOfferRequestInDb is not null)
            {
                return await ResponseWrapper<SparepartsOfferRequestResponse>.SuccessAsync(data: SparepartsOfferRequestInDb);
            }

            return await ResponseWrapper<SparepartsOfferRequestResponse>.SuccessAsync(message: "Offer Request Process does not exists.");
        }
    }
}
