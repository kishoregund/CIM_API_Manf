using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetCurrenciesQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetCurrenciesQueryHandler(ICurrencyService CurrencyService) : IRequestHandler<GetCurrenciesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var CurrencysInDb = await CurrencyService.GetCurrenciesAsync();

            if (CurrencysInDb.Count > 0)
            {
                return await ResponseWrapper<List<CurrencyResponse>>.SuccessAsync(data: CurrencysInDb.Adapt<List<CurrencyResponse>>());
            }
            return await ResponseWrapper<List<CurrencyResponse>>.SuccessAsync(message: "No Currencys were found.");
        }
    }
}
