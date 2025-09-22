using Application.Features.Distributors.Requests;

namespace Application.Features.Distributors.Commands
{
    public class UpdateRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public RegionRequest RegionRequest { get; set; }
    }

    public class UpdateRegionCommandHandler(IRegionService regionService) : IRequestHandler<UpdateRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            var regionInDb = await regionService.GetRegionAsync(request.RegionRequest.Id);

            regionInDb.Region = request.RegionRequest.Region;
            regionInDb.DistRegName = request.RegionRequest.DistRegName;
            regionInDb.DistId = request.RegionRequest.DistId;
            regionInDb.IsBlocked = request.RegionRequest.IsBlocked;
            regionInDb.IsPrincipal = request.RegionRequest.IsPrincipal;
            regionInDb.PayTerms = request.RegionRequest.PayTerms;
            regionInDb.Countries = request.RegionRequest.Countries;

            regionInDb.IsActive = request.RegionRequest.IsActive;
            regionInDb.UpdatedBy = Guid.Empty;
            regionInDb.UpdatedOn = DateTime.Now;
            regionInDb.Area = request.RegionRequest.Area;
            regionInDb.City = request.RegionRequest.City;
            regionInDb.AddrCountryId = request.RegionRequest.AddrCountryId;
            regionInDb.GeoLat = request.RegionRequest.GeoLat;
            regionInDb.GeoLong = request.RegionRequest.GeoLong;
            regionInDb.Place = request.RegionRequest.Place;
            regionInDb.Street = request.RegionRequest.Street;
            regionInDb.Zip = request.RegionRequest.Zip;

            var updateRegionId = await regionService.UpdateRegionAsync(regionInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateRegionId, message: "Record updated successfully.");
        }
    }
}