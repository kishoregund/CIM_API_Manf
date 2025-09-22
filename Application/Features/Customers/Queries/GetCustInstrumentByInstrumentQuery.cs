

using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustInstrumentByInstrumentQuery : IRequest<IResponseWrapper>
    {
        public Guid InstrumentId { get; set; }
    }

    public class GetCustInstrumentByInstrumentQueryHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<GetCustInstrumentByInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustInstrumentByInstrumentQuery request, CancellationToken cancellationToken)
        {
            var custInstrumentInDb = (await CustInstrumentService.GetCustomerInstrumentByInstrumentAsync(request.InstrumentId)).Adapt<CustomerInstrumentResponse>();

            if (custInstrumentInDb is not null)
            {
                return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(data: custInstrumentInDb);
            }
            return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(message: "CustInstrument does not exists.");
        }
    }
}
