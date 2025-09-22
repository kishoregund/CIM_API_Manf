
namespace Application.Features.Travels.Commands
{
    public class DeleteBankDetailsByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }
    public class DeleteBankDetailsByIdCommandHandler(IBankDetailsService BankDetailsService)
        : IRequestHandler<DeleteBankDetailsByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteBankDetailsByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await BankDetailsService.DeleteBankDetailsAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "Bank Details deleted successfully.");
        }
    }
}