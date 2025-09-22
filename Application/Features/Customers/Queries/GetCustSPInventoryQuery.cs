using Application.Features.Customers;
using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustSPInventoryQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class GetCustSPInventoryQueryHandler(ICustSPInventoryService CustSPInventoryService) : IRequestHandler<GetCustSPInventoryQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustSPInventoryQuery request, CancellationToken cancellationToken)
        {
            var CustSPInventoryInDb = await CustSPInventoryService.GetCustSPInventorysAsync(request.ContactId, request.CustomerId);

            if (CustSPInventoryInDb.Count > 0)
            {
                return await ResponseWrapper<List<CustSPInventoryResponse>>.SuccessAsync(data: CustSPInventoryInDb.Adapt<List<CustSPInventoryResponse>>());
            }
            return await ResponseWrapper<List<CustSPInventoryResponse>>.SuccessAsync(message: "No Customer Sparepart Inventory found.");
        }
    }
}
