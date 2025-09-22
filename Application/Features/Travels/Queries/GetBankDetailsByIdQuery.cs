using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetBankDetailsByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }

    public class GetBankDetailsByIdQueryHandler(IBankDetailsService BankDetailservice) : IRequestHandler<GetBankDetailsByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBankDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var BankDetailsInDb = await BankDetailservice.GetBankDetailsByIdAsync(request.Id);

            if (BankDetailsInDb != null)
            {
                return await ResponseWrapper<BankDetails>.SuccessAsync(data: BankDetailsInDb);
            }
            return await ResponseWrapper<BankDetails>.SuccessAsync(message: "No BankDetails were found.");
        }
    }
}