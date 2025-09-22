using Application.Features.AppBasic.Responses;
using Application.Features.Travels;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetBankDetailsByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }

    public class GetBankDetailsByContactIdQueryHandler(IBankDetailsService BankDetailsService) : IRequestHandler<GetBankDetailsByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBankDetailsByContactIdQuery request, CancellationToken cancellationToken)
        {
            var BankDetailsInDb = (await BankDetailsService.GetBankDetailsByContactIdAsync(request.ContactId));

            if (BankDetailsInDb is not null)
            {
                return await ResponseWrapper<BankDetails>.SuccessAsync(data: BankDetailsInDb);
            }
            return await ResponseWrapper<BankDetails>.SuccessAsync(message: "Bank Details does not exists.");
        }
    }
}