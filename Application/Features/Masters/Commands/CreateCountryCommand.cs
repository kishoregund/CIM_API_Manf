using Application.Features.Masters.Requests;
using Application.Features.Masters.Responses;

namespace Application.Features.Masters.Commands
{
    public class CreateCountryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CountryRequest CountryRequest { get; set; }
    }

    public class CreateCountryCommandHandler(ICountryService CountryService) : IRequestHandler<CreateCountryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            // map

            var newCountry = request.CountryRequest.Adapt<Country>();

            var CountryId = await CountryService.CreateCountryAsync(newCountry);

            return await ResponseWrapper<Guid>.SuccessAsync(data: CountryId, message: "Record saved successfully.");
        }
    }
}
