using Application.Features.Customers;
using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustSPInventoryBySRPIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetCustSPInventoryBySRPIdQueryHandler(ICustSPInventoryService CustSPInventoryService) : IRequestHandler<GetCustSPInventoryBySRPIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustSPInventoryBySRPIdQuery request, CancellationToken cancellationToken)
        {
            var CustSPInventoryInDb = (await CustSPInventoryService.GetCustSPInventoryForServiceReportAsync(request.ServiceReportId));//.Adapt<CustSPInventoryResponse>();

            if (CustSPInventoryInDb is not null)
            {
                return await ResponseWrapper<List<CustSPInventoryResponse>>.SuccessAsync(data: CustSPInventoryInDb);
            }
            return await ResponseWrapper<CustSPInventoryResponse>.SuccessAsync(message: "CustSPInventory does not exists.");
        }
    }
}
