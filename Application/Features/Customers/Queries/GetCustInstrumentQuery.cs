using Application.Features.Customers.Responses;
using Application.Models;

namespace Application.Features.Customers.Queries
{
    public class GetCustInstrumentQuery : IRequest<IResponseWrapper>
    {
        public BUBrand BUBrand { get; set; }
    }

    public class GetCustInstrumentsQueryHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<GetCustInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustInstrumentQuery request, CancellationToken cancellationToken)
        {
            var CustInstrumentsInDb = await CustInstrumentService.GetAssignedCustomerInstrumentsAsync(request.BUBrand.BusinessUnitId, request.BUBrand.BrandId);

            if (CustInstrumentsInDb.Count > 0)
            {
                return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(data: CustInstrumentsInDb.Adapt<List<CustomerInstrumentResponse>>());
            }
            return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(message: "No CustInstruments were found.");
        }
    }
}
