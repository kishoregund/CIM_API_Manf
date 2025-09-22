using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class UpdateSparepartsOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SparepartOfferRequestRequest SparepartsOfferRequestRequest { get; set; }
    }

    public class UpdateSparepartsOfferRequestCommandHandler(ISparepartsOfferRequestService SparepartsOfferRequestService)
        : IRequestHandler<UpdateSparepartsOfferRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSparepartsOfferRequestCommand request, CancellationToken cancellationToken)
        {
            var SparepartsOfferRequestInDb = await SparepartsOfferRequestService.GetSparepartsOfferRequestEntityAsync(request.SparepartsOfferRequestRequest.Id);
            
            SparepartsOfferRequestInDb.Id = request.SparepartsOfferRequestRequest.Id;
            SparepartsOfferRequestInDb.Amount = request.SparepartsOfferRequestRequest.Amount;
            SparepartsOfferRequestInDb.PartNo = request.SparepartsOfferRequestRequest.PartNo;
            SparepartsOfferRequestInDb.SparePartId = request.SparepartsOfferRequestRequest.CurrencyId;
            SparepartsOfferRequestInDb.CountryId = request.SparepartsOfferRequestRequest.CountryId;
            SparepartsOfferRequestInDb.OfferRequestId= request.SparepartsOfferRequestRequest.OfferRequestId;
            SparepartsOfferRequestInDb.HsCode = request.SparepartsOfferRequestRequest.HsCode;
            SparepartsOfferRequestInDb.IsActive = request.SparepartsOfferRequestRequest.IsActive;
            SparepartsOfferRequestInDb.IsDeleted = request.SparepartsOfferRequestRequest.IsDeleted;
            SparepartsOfferRequestInDb.PartNo = request.SparepartsOfferRequestRequest.PartNo;
            SparepartsOfferRequestInDb.Price = request.SparepartsOfferRequestRequest.Price;
            SparepartsOfferRequestInDb.Qty = request.SparepartsOfferRequestRequest.Qty;
            SparepartsOfferRequestInDb.DiscountPercentage = request.SparepartsOfferRequestRequest.DiscountPercentage;
            SparepartsOfferRequestInDb.AfterDiscount = request.SparepartsOfferRequestRequest.AfterDiscount;


            var updateSparepartsOfferRequestId = await SparepartsOfferRequestService.UpdateSparepartsOfferRequestAsync(SparepartsOfferRequestInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSparepartsOfferRequestId,
                message: "Record updated successfully.");
        }
    }
}