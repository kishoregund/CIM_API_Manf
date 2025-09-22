using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentAccessoryByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentAccessoryId { get; set; }
    }

    public class GetInstrumentAccessoryByIdQueryHandler(IInstrumentAccessoryService InstrumentAccessoryService) : IRequestHandler<GetInstrumentAccessoryByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentAccessoryByIdQuery request, CancellationToken cancellationToken)
        {
            var instrumentAccessoryInDb = (await InstrumentAccessoryService.GetInstrumentAccessoryAsync(request.InstrumentAccessoryId)).Adapt<InstrumentAccessoryResponse>();

            if (instrumentAccessoryInDb is not null)
            {
                return await ResponseWrapper<InstrumentAccessoryResponse>.SuccessAsync(data: instrumentAccessoryInDb);
            }
            return await ResponseWrapper<InstrumentAccessoryResponse>.SuccessAsync(message: "InstrumentAccessory does not exists.");
        }
    }
}
