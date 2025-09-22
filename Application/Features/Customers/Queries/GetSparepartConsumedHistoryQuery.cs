using Application.Features.Customers;
using Application.Features.Customers.Responses;
using Domain.Views;

namespace Application.Features.Customers.Queries
{
    public class GetSparepartConsumedHistoryQuery : IRequest<IResponseWrapper>
    {
        public Guid CustSPInventoryId { get; set; }
    }

    public class GetSparepartConsumedHistoryQueryHandler(ICustSPInventoryService CustSPInventoryService) : IRequestHandler<GetSparepartConsumedHistoryQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSparepartConsumedHistoryQuery request, CancellationToken cancellationToken)
        {
            var CustSPInventoryInDb = (await CustSPInventoryService.GetSparepartConsumedHistoryAsync(request.CustSPInventoryId)).Adapt<List<VW_SparepartConsumedHistory>>();

            if (CustSPInventoryInDb is not null)
            {
                return await ResponseWrapper<List<VW_SparepartConsumedHistory>>.SuccessAsync(data: CustSPInventoryInDb);
            }
            return await ResponseWrapper<VW_SparepartConsumedHistory>.SuccessAsync(message: "History does not exists.");
        }
    }
}
