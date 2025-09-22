
using Application.Features.Customers.Requests;
using Application.Features.Identity.Users.Queries;
using Domain.Entities;

namespace Application.Features.Customers.Commands
{    
    public class UpdateCustSPInventoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustSPInventoryRequest CustSPInventoryRequest { get; set; }
    }

    public class UpdateCustSPInventoryCommandHandler(ICustSPInventoryService CustSpInventoryService) : IRequestHandler<UpdateCustSPInventoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCustSPInventoryCommand request, CancellationToken cancellationToken)
        {
            var CustSpInventoryInDb = await CustSpInventoryService.GetCustSPInventoryEntityAsync(request.CustSPInventoryRequest.Id);

            CustSpInventoryInDb.Id = request.CustSPInventoryRequest.Id;
            CustSpInventoryInDb.CustomerId = request.CustSPInventoryRequest.CustomerId;
            CustSpInventoryInDb.InstrumentId = request.CustSPInventoryRequest.InstrumentId;
            CustSpInventoryInDb.QtyAvailable = request.CustSPInventoryRequest.QtyAvailable;
            CustSpInventoryInDb.SiteId = request.CustSPInventoryRequest.SiteId;
            CustSpInventoryInDb.SparePartId = request.CustSPInventoryRequest.SparePartId;
            CustSpInventoryInDb.UpdatedBy = request.CustSPInventoryRequest.UpdatedBy;

            var updateCustSPInventoryId = await CustSpInventoryService.UpdateCustSPInventoryAsync(CustSpInventoryInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCustSPInventoryId, message: "Record updated successfully.");
        }
    }
}