using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetCurrencyByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid CurrencyId { get; set; }
    }

    public class GetCurrencyByIdQueryHandler(ICurrencyService CurrencyService) : IRequestHandler<GetCurrencyByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            var currencyInDb = (await CurrencyService.GetCurrencyAsync(request.CurrencyId)).Adapt<CurrencyResponse>();

            if (currencyInDb is not null)
            {
                return await ResponseWrapper<CurrencyResponse>.SuccessAsync(data: currencyInDb);
            }
            return await ResponseWrapper<CurrencyResponse>.SuccessAsync(message: "Currency does not exists.");
        }
    }
}
