using Application.Features.Manufacturers.Commands;
using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class UpdateManufacturerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ManufacturerRequest ManufacturerRequest { get; set; }
    }

    public class UpdateManufacturerCommandHandler(IManufacturerService ManufacturerService) : IRequestHandler<UpdateManufacturerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
        {
            var ManufacturerInDb = await ManufacturerService.GetManufacturerAsync(request.ManufacturerRequest.Id);

            ManufacturerInDb.ManfName = request.ManufacturerRequest.ManfName;
            ManufacturerInDb.IsBlocked = request.ManufacturerRequest.IsBlocked;
            ManufacturerInDb.IsActive = request.ManufacturerRequest.IsActive;
            ManufacturerInDb.Payterms = request.ManufacturerRequest.Payterms;
            ManufacturerInDb.UpdatedBy = Guid.Empty;
            ManufacturerInDb.UpdatedOn = DateTime.Now;
            ManufacturerInDb.Area = request.ManufacturerRequest.Area;
            ManufacturerInDb.City = request.ManufacturerRequest.City;
            ManufacturerInDb.AddrCountryId = request.ManufacturerRequest.AddrCountryId;
            ManufacturerInDb.GeoLat = request.ManufacturerRequest.GeoLat;
            ManufacturerInDb.GeoLong = request.ManufacturerRequest.GeoLong;
            ManufacturerInDb.Place = request.ManufacturerRequest.Place;
            ManufacturerInDb.Street = request.ManufacturerRequest.Street;
            ManufacturerInDb.Zip = request.ManufacturerRequest.Zip;

            var updateManufacturerId = await ManufacturerService.UpdateManufacturerAsync(ManufacturerInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateManufacturerId, message: "Record updated successfully.");
        }
    }
}
