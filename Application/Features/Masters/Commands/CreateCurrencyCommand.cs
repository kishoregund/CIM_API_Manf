using Application.Features.Masters.Requests;
using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Commands
{
    public class CreateCurrencyCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CurrencyRequest CurrencyRequest { get; set; }
    }

    public class CreateCurrencyCommandHandler(ICurrencyService CurrencyService) : IRequestHandler<CreateCurrencyCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            // map

            var newCurrency = request.CurrencyRequest.Adapt<Currency>();

            var CurrencyId = await CurrencyService.CreateCurrencyAsync(newCurrency);

            return await ResponseWrapper<Guid>.SuccessAsync(data: CurrencyId, message: "Record saved successfully.");
        }
    }
}
