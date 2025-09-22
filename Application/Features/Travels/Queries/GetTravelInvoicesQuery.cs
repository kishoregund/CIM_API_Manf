using Application.Features.AppBasic.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetTravelInvoicesQuery : IRequest<IResponseWrapper>
    {
        public string BusinessUnitId { get; set; }
        public string BrandId { get; set; }
    }
    public class GetTravelInvoicesQueryHandler(ITravelInvoiceService TravelInvoiceService) : IRequestHandler<GetTravelInvoicesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTravelInvoicesQuery request, CancellationToken cancellationToken)
        {
            var TravelInvoiceInDb = (await TravelInvoiceService.GetTravelInvoicesAsync(request.BusinessUnitId, request.BrandId)).Adapt<List<TravelInvoiceResponse>>();

            if (TravelInvoiceInDb is not null)
            {
                return await ResponseWrapper<List<TravelInvoiceResponse>>.SuccessAsync(data: TravelInvoiceInDb);
            }
            return await ResponseWrapper<TravelInvoiceResponse>.SuccessAsync(message: "TravelInvoice does not exists.");
        }
    }
}