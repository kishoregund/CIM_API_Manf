using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class UpdateOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public OfferRequestRequest OfferRequestRequest { get; set; }
    }

    public class UpdateOfferRequestCommandHandler(IOfferRequestService OfferRequestService)
        : IRequestHandler<UpdateOfferRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateOfferRequestCommand request, CancellationToken cancellationToken)
        {
            var OfferRequestInDb = await OfferRequestService.GetOfferRequestEntityAsync(request.OfferRequestRequest.Id);
            OfferRequestInDb.Id = request.OfferRequestRequest.Id;
            OfferRequestInDb.AirFreightChargesAmt = request.OfferRequestRequest.AirFreightChargesAmt;
            OfferRequestInDb.AirFreightChargesCurr = request.OfferRequestRequest.AirFreightChargesCurr;
            OfferRequestInDb.CurrencyId = request.OfferRequestRequest.CurrencyId;
            OfferRequestInDb.BasePCurrencyAmt = request.OfferRequestRequest.BasePCurrencyAmt;
            OfferRequestInDb.CustomerId = request.OfferRequestRequest.CustomerId;
            OfferRequestInDb.CustomerSiteId = request.OfferRequestRequest.CustomerSiteId;
            OfferRequestInDb.DistributorId = request.OfferRequestRequest.DistributorId;
            OfferRequestInDb.InspectionChargesAmt = request.OfferRequestRequest.InspectionChargesAmt;
            OfferRequestInDb.InspectionChargesCurr = request.OfferRequestRequest.InspectionChargesCurr;
            OfferRequestInDb.Instruments = request.OfferRequestRequest.InstrumentsList;
            OfferRequestInDb.IsActive = request.OfferRequestRequest.IsActive;
            OfferRequestInDb.IsDeleted = request.OfferRequestRequest.IsDeleted;
            OfferRequestInDb.IsDistUpdated = request.OfferRequestRequest.IsDistUpdated;
            OfferRequestInDb.LcAdministrativeChargesAmt = request.OfferRequestRequest.LcAdministrativeChargesAmt;
            OfferRequestInDb.LcadministrativeChargesCurr = request.OfferRequestRequest.LcadministrativeChargesCurr;
            OfferRequestInDb.OffReqNo = request.OfferRequestRequest.OffReqNo;
            OfferRequestInDb.OtherSpareDesc = request.OfferRequestRequest.OtherSpareDesc;
            OfferRequestInDb.PaymentTerms = request.OfferRequestRequest.PaymentTerms;
            OfferRequestInDb.PoDate = request.OfferRequestRequest.PoDate;
            OfferRequestInDb.SpareQuoteNo = request.OfferRequestRequest.SpareQuoteNo;
            OfferRequestInDb.Status = request.OfferRequestRequest.Status;
            OfferRequestInDb.TotalAmount = request.OfferRequestRequest.TotalAmount;
            OfferRequestInDb.TotalAmt = request.OfferRequestRequest.TotalAmt;
            OfferRequestInDb.TotalCurr = request.OfferRequestRequest.TotalCurr;

            var updateOfferRequestId = await OfferRequestService.UpdateOfferRequestAsync(OfferRequestInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateOfferRequestId,
                message: "Record updated successfully.");
        }
    }
}