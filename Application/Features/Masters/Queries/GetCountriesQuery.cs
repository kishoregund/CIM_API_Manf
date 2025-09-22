using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetCountriesQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetCountriesQueryHandler(ICountryService CountryService) : IRequestHandler<GetCountriesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var CountrysInDb = await CountryService.GetCountriesAsync();

            if (CountrysInDb.Count > 0)
            {
                return await ResponseWrapper<List<CountryResponse>>.SuccessAsync(data: CountrysInDb.Adapt<List<CountryResponse>>());
            }
            return await ResponseWrapper<List<CountryResponse>>.SuccessAsync(message: "No Countrys were found.");
        }
    }
}
