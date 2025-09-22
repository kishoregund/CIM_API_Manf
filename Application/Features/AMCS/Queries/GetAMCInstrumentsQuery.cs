using Application.Exceptions;
using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS.Queries
{
    public class GetAmcInstrumentsQuery : IRequest<IResponseWrapper>
    {
        public Guid AmcId { get; set; }
    }

    public class GetAmcInstrumentsQueryHandler(IAmcInstrumentService amcInstrumentService)
        : IRequestHandler<GetAmcInstrumentsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAmcInstrumentsQuery request, CancellationToken cancellationToken)
        {
            var amcInsInDb = (await amcInstrumentService.GetByAmcIdAsync(request.AmcId));//.Adapt<List<AmcInstrumentResponse>>();

            if (amcInsInDb is not null)
            {
                return await ResponseWrapper<List<AmcInstrumentResponse>>.SuccessAsync(data: amcInsInDb);
            }
            return await ResponseWrapper<AmcInstrumentResponse>.SuccessAsync(message: "AMC Instruments does not exists.");
        }
    }
}