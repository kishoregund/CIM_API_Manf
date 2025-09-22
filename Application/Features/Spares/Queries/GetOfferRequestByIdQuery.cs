using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetOfferRequestByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetOfferRequestByIdQueryHandler(IOfferRequestService OfferRequestService)
        : IRequestHandler<GetOfferRequestByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetOfferRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var OfferRequestInDb = (await OfferRequestService.GetOfferRequestAsync(request.Id)).Adapt<OfferRequestResponse>();

            if (OfferRequestInDb is not null)
            {
                return await ResponseWrapper<OfferRequestResponse>.SuccessAsync(data: OfferRequestInDb);
            }

            return await ResponseWrapper<OfferRequestResponse>.SuccessAsync(message: "OfferRequest does not exists.");
        }
    }
}
