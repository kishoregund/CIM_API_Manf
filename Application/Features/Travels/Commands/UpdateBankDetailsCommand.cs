using Application.Features.AppBasic.Requests;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class UpdateBankDetailsCommand : IRequest<IResponseWrapper>
    {
        public BankDetails Request { get; set; }
    }

    public class UpdateBankDetailsCommandHandler(IBankDetailsService BankDetailsService)
        : IRequestHandler<UpdateBankDetailsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateBankDetailsCommand request, CancellationToken cancellationToken)
        {
            var BankDetails = await BankDetailsService.GetBankDetailsEntityByIdAsync(request.Request.Id);

            BankDetails.Id = request.Request.Id;
            BankDetails.IsActive = request.Request.IsActive;
            BankDetails.BankAccountNo = request.Request.BankAccountNo;
            BankDetails.Branch = request.Request.Branch;
            BankDetails.BankName = request.Request.BankName;
            BankDetails.BankSwiftCode = request.Request.BankSwiftCode;
            BankDetails.NameInBank = request.Request.NameInBank;
            BankDetails.ContactId = request.Request.ContactId;
            BankDetails.IBANNo = request.Request.IBANNo;

            var result = await BankDetailsService.UpdateBankDetailsAsync(BankDetails);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}