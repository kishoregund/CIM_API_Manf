namespace Application.Features.Masters.Commands
{
    public class DeleteCurrencyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid CurrencyId { get; set; }
    }

    public class DeleteCurrencyCommandHandler(ICurrencyService CurrencyService) : IRequestHandler<DeleteCurrencyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var deletedCurrency = await CurrencyService.DeleteCurrencyAsync(request.CurrencyId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedCurrency, message: "Currency deleted successfully.");
        }
    }
}
