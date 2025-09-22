using Application.Features.Customers;
using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustSPInventoryByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid CustSPInventoryId { get; set; }
    }

    public class GetCustSPInventoryByIdQueryHandler(ICustSPInventoryService CustSPInventoryService) : IRequestHandler<GetCustSPInventoryByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustSPInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var CustSPInventoryInDb = (await CustSPInventoryService.GetCustSPInventoryAsync(request.CustSPInventoryId));//.Adapt<CustSPInventoryResponse>();

            if (CustSPInventoryInDb is not null)
            {
                return await ResponseWrapper<CustSPInventoryResponse>.SuccessAsync(data: CustSPInventoryInDb);
            }
            return await ResponseWrapper<CustSPInventoryResponse>.SuccessAsync(message: "CustSPInventory does not exists.");
        }
    }
}
