using Application.Features.Customers.Requests;

namespace Application.Features.Customers.Commands
{
    public class UpdateCustInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerInstrumentRequest CustInstrumentRequest { get; set; }
    }

    public class UpdateCustInstrumentCommandHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<UpdateCustInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCustInstrumentCommand request, CancellationToken cancellationToken)
        {
            var CustInstrumentInDb = await CustInstrumentService.GetCustomerInstrumentAsync(request.CustInstrumentRequest.Id);

            CustInstrumentInDb.Id = request.CustInstrumentRequest.Id;
            CustInstrumentInDb.BaseCurrencyAmt = request.CustInstrumentRequest.BaseCurrencyAmt;
            CustInstrumentInDb.InstrumentId= request.CustInstrumentRequest.InstrumentId;
            CustInstrumentInDb.Cost = request.CustInstrumentRequest.Cost;
            CustInstrumentInDb.CurrencyId = request.CustInstrumentRequest.CurrencyId;
            CustInstrumentInDb.CustSiteId = request.CustInstrumentRequest.CustSiteId;
            CustInstrumentInDb.DateOfPurchase = request.CustInstrumentRequest.DateOfPurchase;
            CustInstrumentInDb.EngContact = request.CustInstrumentRequest.EngContact;
            CustInstrumentInDb.EngEmail = request.CustInstrumentRequest.EngEmail;
            CustInstrumentInDb.EngName = request.CustInstrumentRequest.EngName;
            CustInstrumentInDb.EngNameOther = request.CustInstrumentRequest.EngNameOther;
            CustInstrumentInDb.InstallBy = request.CustInstrumentRequest.InstallBy;
            CustInstrumentInDb.InstallByOther = request.CustInstrumentRequest.InstallByOther;
            CustInstrumentInDb.InstallDt = request.CustInstrumentRequest.InstallDt;
            CustInstrumentInDb.InstruEngineerId = request.CustInstrumentRequest.InstruEngineerId;
            CustInstrumentInDb.OperatorId = request.CustInstrumentRequest.OperatorId;
            CustInstrumentInDb.InsMfgDt = request.CustInstrumentRequest.InsMfgDt;
            CustInstrumentInDb.ShipDt = request.CustInstrumentRequest.ShipDt;
            CustInstrumentInDb.Warranty = request.CustInstrumentRequest.Warranty;
            CustInstrumentInDb.WrntyEnDt = request.CustInstrumentRequest.WrntyEnDt;
            CustInstrumentInDb.WrntyStDt = request.CustInstrumentRequest.WrntyStDt;
            CustInstrumentInDb.UpdatedBy = request.CustInstrumentRequest.UpdatedBy;

            var updateCustInstrumentId = await CustInstrumentService.UpdateCustomerInstrumentAsync(CustInstrumentInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCustInstrumentId, message: "Record updated successfully.");
        }
    }
}

