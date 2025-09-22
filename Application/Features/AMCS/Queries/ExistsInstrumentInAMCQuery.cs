using Application.Exceptions;
using Application.Features.AMCS.Requests;
using Application.Features.AMCS.Responses;

namespace Application.Features.AMCS.Queries
{
    public class ExistsInstrumentInAMCQuery : IRequest<IResponseWrapper>
    {
        public AmcRequest AMC { get; set; }
    }

    public class ExistsInstrumentInAMCQueryHandler(IAmcInstrumentService amcInstrumentService)
        : IRequestHandler<ExistsInstrumentInAMCQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(ExistsInstrumentInAMCQuery request, CancellationToken cancellationToken)
        {
            var response = await amcInstrumentService.ExistsInstrumentInAMCQuery(request.AMC);

            return await ResponseWrapper<bool>.SuccessAsync(data: response);
        }
    }
}