using Application.Features.AppBasic.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetTravelInvoiceByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid TravelInvoiceId { get; set; }
    }
    public class GetTravelInvoiceByIdQueryHandler(ITravelInvoiceService TravelInvoiceService) : IRequestHandler<GetTravelInvoiceByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
                var TravelInvoiceInDb = (await TravelInvoiceService.GetTravelInvoiceByIdAsync(request.TravelInvoiceId)).Adapt<TravelInvoiceResponse>();

                if (TravelInvoiceInDb is not null)
                {
                    return await ResponseWrapper<TravelInvoiceResponse>.SuccessAsync(data: TravelInvoiceInDb);
                }
            return await ResponseWrapper<TravelInvoiceResponse>.SuccessAsync(message: "TravelInvoice does not exists.");
        }
    }
}