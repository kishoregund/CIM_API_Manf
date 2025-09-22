using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Queries
{
    public class GetCountryByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid CountryId { get; set; }
    }

    public class GetCountryByIdQueryHandler(ICountryService CountryService) : IRequestHandler<GetCountryByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var countryInDb = (await CountryService.GetCountryAsync(request.CountryId)).Adapt<CountryResponse>();

            if (countryInDb is not null)
            {
                return await ResponseWrapper<CountryResponse>.SuccessAsync(data: countryInDb);
            }
            return await ResponseWrapper<CountryResponse>.SuccessAsync(message: "Country does not exists.");
        }
    }
}
