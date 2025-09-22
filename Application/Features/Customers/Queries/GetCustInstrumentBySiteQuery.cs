

using Application.Features.Customers.Responses;
using Application.Features.Instruments.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustInstrumentBySiteQuery : IRequest<IResponseWrapper>
    {
        public Guid SiteId { get; set; }
    }

    public class GetCustInstrumentBySiteQueryHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<GetCustInstrumentBySiteQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustInstrumentBySiteQuery request, CancellationToken cancellationToken)
        {
            var custInstrumentInDb = (await CustInstrumentService.GetCustomersInstrumentBySiteAsync(request.SiteId)).Adapt<List<InstrumentResponse>>();

            if (custInstrumentInDb is not null)
            {
                return await ResponseWrapper<List<InstrumentResponse>>.SuccessAsync(data: custInstrumentInDb);
            }
            return await ResponseWrapper<InstrumentResponse>.SuccessAsync(message: "Customer's Instrument does not exists.");
        }
    }
}
