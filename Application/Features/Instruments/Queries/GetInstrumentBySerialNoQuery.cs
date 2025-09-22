using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentBySerialNoQuery : IRequest<IResponseWrapper>
    {
        public string serialNo { get; set; }
    }

    public class GetInstrumentBySerialNoQueryHandler(IInstrumentService InstrumentService, IInstrumentSparesService instrumentSparesService) : IRequestHandler<GetInstrumentBySerialNoQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentBySerialNoQuery request, CancellationToken cancellationToken)
        {
            var instrumentInDb = (await InstrumentService.GetInstrumentBySerialNoAsync(request.serialNo)).Adapt<InstrumentResponse>();

            if (instrumentInDb is not null)
            {
                instrumentInDb.Spares = (await instrumentSparesService.GetInstrumentSparesByInsIdAsync(instrumentInDb.Id)).Adapt<List<InstrumentSparesResponse>>();
                return await ResponseWrapper<InstrumentResponse>.SuccessAsync(data: instrumentInDb);
            }
            return await ResponseWrapper<InstrumentResponse>.SuccessAsync(message: "Instrument does not exists.");
        }
    }
}
