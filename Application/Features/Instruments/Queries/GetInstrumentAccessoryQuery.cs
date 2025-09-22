

using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentAccessoryQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
    }

    public class GetInstrumentAccessoryQueryHandler(IInstrumentAccessoryService InstrumentAccessoryervice) : IRequestHandler<GetInstrumentAccessoryQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentAccessoryQuery request, CancellationToken cancellationToken)
        {
            var InstrumentAccessoryInDb = await InstrumentAccessoryervice.GetInstrumentAccessoryByInsIdAsync(request.InstrumentId);

            if (InstrumentAccessoryInDb.Count > 0)
            {
                return await ResponseWrapper<List<InstrumentAccessoryResponse>>.SuccessAsync(data: InstrumentAccessoryInDb.Adapt<List<InstrumentAccessoryResponse>>());
            }
            return await ResponseWrapper<List<InstrumentAccessoryResponse>>.SuccessAsync(message: "No InstrumentAccessory were found.");
        }
    }
}
