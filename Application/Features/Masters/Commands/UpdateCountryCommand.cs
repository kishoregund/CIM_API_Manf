using Application.Features.Masters.Requests;
using Domain.Entities;
using System.Xml.Linq;

namespace Application.Features.Masters.Commands
{
    public class UpdateCountryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CountryRequest CountryRequest { get; set; }
    }

    public class UpdateCountryCommandHandler(ICountryService CountryService) : IRequestHandler<UpdateCountryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var CountryInDb = await CountryService.GetCountryAsync(request.CountryRequest.Id);

            CountryInDb.Id = request.CountryRequest.Id;
            CountryInDb.Capital = request.CountryRequest.Capital;
            CountryInDb.ContinentId = request.CountryRequest.ContinentId;
            CountryInDb.CurrencyId = request.CountryRequest.CurrencyId;
            CountryInDb.Formal = request.CountryRequest.Formal;
            CountryInDb.Iso_2 = request.CountryRequest.Iso_2;
            CountryInDb.Iso_3 = request.CountryRequest.Iso_3;
            CountryInDb.Name = request.CountryRequest.Name;
            CountryInDb.Region = request.CountryRequest.Region;
            CountryInDb.Sub_Region = request.CountryRequest.Sub_Region;
            CountryInDb.UpdatedBy = request.CountryRequest.UpdatedBy;


            var updateCountryId = await CountryService.UpdateCountryAsync(CountryInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCountryId, message: "Record updated successfully.");
        }
    }
}