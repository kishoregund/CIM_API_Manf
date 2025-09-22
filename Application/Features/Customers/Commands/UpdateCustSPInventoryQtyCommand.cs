
using Application.Features.Customers.Requests;
using Domain.Entities;

namespace Application.Features.Customers.Commands
{
    public class UpdateCustSPInventoryQtyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid Id { get; set; }
        public int Qty { get; set; }
    }

    public class UpdateCustSPInventoryQtyCommandHandler(ICustSPInventoryService CustSpInventoryService) : IRequestHandler<UpdateCustSPInventoryQtyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCustSPInventoryQtyCommand request, CancellationToken cancellationToken)
        {
            var CustSpInventoryInDb = await CustSpInventoryService.GetCustSPInventoryEntityAsync(request.Id);

            CustSpInventoryInDb.QtyAvailable = request.Qty;

            var updateCustSPInventoryId = await CustSpInventoryService.UpdateCustSPInventoryAsync(CustSpInventoryInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCustSPInventoryId, message: "Record updated successfully.");
        }
    }


}