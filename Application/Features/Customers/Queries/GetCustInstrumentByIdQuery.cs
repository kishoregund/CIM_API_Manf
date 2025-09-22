

using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustInstrumentByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid CustInstrumentId { get; set; }
    }

    public class GetCustInstrumentByIdQueryHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<GetCustInstrumentByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustInstrumentByIdQuery request, CancellationToken cancellationToken)
        {
            var custInstrumentInDb = (await CustInstrumentService.GetCustomerInstrumentAsync(request.CustInstrumentId)).Adapt<CustomerInstrumentResponse>();

            if (custInstrumentInDb is not null)
            {
                return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(data: custInstrumentInDb);
            }
            return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(message: "CustInstrument does not exists.");
        }
    }
}
