using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetOfferRequestProcessByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetOfferRequestProcessByIdQueryHandler(IOfferRequestProcessService OfferRequestProcessService)
        : IRequestHandler<GetOfferRequestProcessByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetOfferRequestProcessByIdQuery request, CancellationToken cancellationToken)
        {
            var OfferRequestProcessInDb = (await OfferRequestProcessService.GetOfferRequestProcessAsync(request.Id)).Adapt<OfferRequestProcessResponse>();

            if (OfferRequestProcessInDb is not null)
            {
                return await ResponseWrapper<OfferRequestProcessResponse>.SuccessAsync(data: OfferRequestProcessInDb);
            }

            return await ResponseWrapper<OfferRequestProcessResponse>.SuccessAsync(message: "Offer Request Process does not exists.");
        }
    }
}
