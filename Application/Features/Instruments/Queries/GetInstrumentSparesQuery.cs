using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Queries
{
    public class GetInstrumentSparesQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
    }

    public class GetInstrumentSparesQueryHandler(IInstrumentSparesService InstrumentSpareservice) : IRequestHandler<GetInstrumentSparesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentSparesQuery request, CancellationToken cancellationToken)
        {
            var InstrumentSparesInDb = await InstrumentSpareservice.GetInstrumentSparesByInsIdAsync(request.InstrumentId);

            if (InstrumentSparesInDb.Count > 0)
            {
                return await ResponseWrapper<List<InstrumentSparesResponse>>.SuccessAsync(data: InstrumentSparesInDb.Adapt<List<InstrumentSparesResponse>>());
            }
            return await ResponseWrapper<List<InstrumentSparesResponse>>.SuccessAsync(message: "No InstrumentSpares were found.");
        }
    }
}
