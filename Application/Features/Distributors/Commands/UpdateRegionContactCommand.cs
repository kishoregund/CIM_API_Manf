using Application.Features.Distributors.Requests;

namespace Application.Features.Distributors.Commands
{
    public class UpdateRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public RegionContactRequest RegionContactRequest { get; set; }
    }

    public class UpdateRegionContactCommandHandler(IRegionContactService regionContactService) : IRequestHandler<UpdateRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateRegionContactCommand request, CancellationToken cancellationToken)
        {
            var regionContactInDb = await regionContactService.GetRegionContactAsync(request.RegionContactRequest.Id);
            regionContactInDb.Id = request.RegionContactRequest.Id;
            regionContactInDb.DesignationId = request.RegionContactRequest.DesignationId;
            regionContactInDb.Zip = request.RegionContactRequest.Zip;
            regionContactInDb.RegionId = request.RegionContactRequest.RegionId;
            regionContactInDb.FirstName = request.RegionContactRequest.FirstName;
            regionContactInDb.LastName = request.RegionContactRequest.LastName;
            regionContactInDb.MiddleName = request.RegionContactRequest.MiddleName;
            regionContactInDb.PrimaryContactNo = request.RegionContactRequest.PrimaryContactNo;
            regionContactInDb.PrimaryEmail = request.RegionContactRequest.PrimaryEmail;
            regionContactInDb.SecondaryContactNo = request.RegionContactRequest.SecondaryContactNo;
            regionContactInDb.SecondaryEmail = request.RegionContactRequest.SecondaryEmail;
            regionContactInDb.WhatsappNo = request.RegionContactRequest.WhatsappNo;
            regionContactInDb.Street = request.RegionContactRequest.Street;
            regionContactInDb.Place = request.RegionContactRequest.Place;
            regionContactInDb.Area = request.RegionContactRequest.Area;
            regionContactInDb.City = request.RegionContactRequest.City;
            regionContactInDb.AddrCountryId = request.RegionContactRequest.AddrCountryId;
            regionContactInDb.GeoLat = request.RegionContactRequest.GeoLat;
            regionContactInDb.GeoLong = request.RegionContactRequest.GeoLong;
            regionContactInDb.IsActive = request.RegionContactRequest.IsActive;
            regionContactInDb.IsFieldEngineer = request.RegionContactRequest.IsFieldEngineer;
            regionContactInDb.UpdatedBy = request.RegionContactRequest.UpdatedBy;


            var updateRegionContactId = await regionContactService.UpdateRegionContactAsync(regionContactInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateRegionContactId, message: "Record updated successfully.");
        }
    }
}