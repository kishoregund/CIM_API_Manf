using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentSparesByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentSparesId { get; set; }
    }

    public class GetInstrumentSparesByIdQueryHandler(IInstrumentSparesService InstrumentSparesService) : IRequestHandler<GetInstrumentSparesByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentSparesByIdQuery request, CancellationToken cancellationToken)
        {
            var InstrumentSparesInDb = (await InstrumentSparesService.GetInstrumentSparesAsync(request.InstrumentSparesId)).Adapt<InstrumentSparesResponse>();

            if (InstrumentSparesInDb is not null)
            {
                return await ResponseWrapper<InstrumentSparesResponse>.SuccessAsync(data: InstrumentSparesInDb);
            }
            return await ResponseWrapper<InstrumentSparesResponse>.SuccessAsync(message: "InstrumentSpares does not exists.");
        }
    }
}
