using Application.Features.Spares.Responses;

namespace Application.Features.Spares.Queries
{
    public class GetSparepartsByInstrumentPartNoQuery : IRequest<IResponseWrapper>
    {
        public string InstrumentIds { get; set; }
        public string PartNo { get; set; }
    }

    public class GetSparepartsByInstrumentPartNoQueryHandler(IOfferRequestService offerRequestService)
        : IRequestHandler<GetSparepartsByInstrumentPartNoQuery, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(GetSparepartsByInstrumentPartNoQuery request, CancellationToken cancellationToken)
        {
            var sparepartInDb = (await offerRequestService.GetSparepartsByInstrumentPartNoAsync(request.InstrumentIds, request.PartNo));

            if (sparepartInDb.Count > 0)
            {
                return await ResponseWrapper<List<SparepartsOfferRequestResponse>>.SuccessAsync(data: sparepartInDb);
            }

            return await ResponseWrapper<SparepartsOfferRequestResponse>.SuccessAsync(message: "No Spareparts By Instrument & PartNo were found.");
        }
    }
}