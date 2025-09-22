
using Application.Features.Distributors.Requests;


namespace Application.Features.Distributors.Commands
{
    public class UpdateDistributorCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public DistributorRequest DistributorRequest { get; set; }
    }

    public class UpdateDistributorCommandHandler(IDistributorService distributorService) : IRequestHandler<UpdateDistributorCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateDistributorCommand request, CancellationToken cancellationToken)
        {
            var distributorInDb = await distributorService.GetDistributorEntityAsync(request.DistributorRequest.Id);

            distributorInDb.DistName = request.DistributorRequest.DistName;
            distributorInDb.IsBlocked = request.DistributorRequest.IsBlocked;
            distributorInDb.IsActive = request.DistributorRequest.IsActive;
            distributorInDb.Payterms = request.DistributorRequest.Payterms;
            distributorInDb.ManufacturerIds = request.DistributorRequest.ManufacturerIds;
            distributorInDb.Area = request.DistributorRequest.Area;
            distributorInDb.City = request.DistributorRequest.City;
            distributorInDb.AddrCountryId = request.DistributorRequest.AddrCountryId;
            distributorInDb.GeoLat = request.DistributorRequest.GeoLat;
            distributorInDb.GeoLong = request.DistributorRequest.GeoLong;
            distributorInDb.Place = request.DistributorRequest.Place;
            distributorInDb.Street = request.DistributorRequest.Street;
            distributorInDb.Zip = request.DistributorRequest.Zip;

            var updateDistributorId = await distributorService.UpdateDistributorAsync(distributorInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateDistributorId, message: "Record updated successfully.");
        }
    }
}