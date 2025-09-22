using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
    }

    public class GetInstrumentByIdQueryHandler(IInstrumentService InstrumentService, IInstrumentSparesService instrumentSparesService,
        IInstrumentAccessoryService instrumentAccessoryService) : IRequestHandler<GetInstrumentByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentByIdQuery request, CancellationToken cancellationToken)
        {
            var instrumentInDb = (await InstrumentService.GetInstrumentAsync(request.InstrumentId)).Adapt<InstrumentResponse>();

            if (instrumentInDb is not null)
            {
                instrumentInDb.Spares = (await instrumentSparesService.GetInstrumentSparesByInsIdAsync(request.InstrumentId)).Adapt<List<InstrumentSparesResponse>>();
                instrumentInDb.Accessories = (await instrumentAccessoryService.GetInstrumentAccessoryByInsIdAsync(request.InstrumentId)).Adapt<List<InstrumentAccessoryResponse>>();
                return await ResponseWrapper<InstrumentResponse>.SuccessAsync(data: instrumentInDb);
            }
            return await ResponseWrapper<InstrumentResponse>.SuccessAsync(message: "Instrument does not exists.");
        }
    }
}
