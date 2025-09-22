

using Application.Features.Customers;
using Application.Features.Customers.Requests;
using Domain.Entities;

namespace Application.Features.Customers.Commands
{
    public class CreateCustSPInventoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustSPInventoryRequest CustSPInventoryRequest { get; set; }
    }

    public class CreateCustSpInventoryCommandHandler(ICustSPInventoryService CustSpInventoryService) : IRequestHandler<CreateCustSPInventoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCustSPInventoryCommand request, CancellationToken cancellationToken)
        {
            // map

            var newCustSpInventory = request.CustSPInventoryRequest.Adapt<CustSPInventory>();

            var CustSpInventoryId = await CustSpInventoryService.CreateCustSPInventoryAsync(newCustSpInventory);

            return await ResponseWrapper<Guid>.SuccessAsync(data: CustSpInventoryId, message: "Record saved successfully.");
        }
    }
}

