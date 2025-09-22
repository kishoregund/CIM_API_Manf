
using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class UpdateSalesRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SalesRegionContactRequest SalesRegionContactRequest { get; set; }
    }

    public class UpdateSalesRegionContactCommandHandler(ISalesRegionContactService SalesRegionContactService) : IRequestHandler<UpdateSalesRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSalesRegionContactCommand request, CancellationToken cancellationToken)
        {
            var SalesRegionContactInDb = await SalesRegionContactService.GetSalesRegionContactAsync(request.SalesRegionContactRequest.Id);

            SalesRegionContactInDb.Id = request.SalesRegionContactRequest.Id;
            SalesRegionContactInDb.DesignationId = request.SalesRegionContactRequest.DesignationId;
            SalesRegionContactInDb.Zip = request.SalesRegionContactRequest.Zip;
            SalesRegionContactInDb.SalesRegionId = request.SalesRegionContactRequest.SalesRegionId;
            SalesRegionContactInDb.FirstName = request.SalesRegionContactRequest.FirstName;
            SalesRegionContactInDb.LastName = request.SalesRegionContactRequest.LastName;
            SalesRegionContactInDb.MiddleName = request.SalesRegionContactRequest.MiddleName;
            SalesRegionContactInDb.PrimaryContactNo = request.SalesRegionContactRequest.PrimaryContactNo;
            SalesRegionContactInDb.PrimaryEmail = request.SalesRegionContactRequest.PrimaryEmail;
            SalesRegionContactInDb.SecondaryContactNo = request.SalesRegionContactRequest.SecondaryContactNo;
            SalesRegionContactInDb.SecondaryEmail = request.SalesRegionContactRequest.SecondaryEmail;
            SalesRegionContactInDb.WhatsappNo = request.SalesRegionContactRequest.WhatsappNo;
            SalesRegionContactInDb.Street = request.SalesRegionContactRequest.Street;
            SalesRegionContactInDb.Place = request.SalesRegionContactRequest.Place;
            SalesRegionContactInDb.Area = request.SalesRegionContactRequest.Area;
            SalesRegionContactInDb.City = request.SalesRegionContactRequest.City;
            SalesRegionContactInDb.AddrCountryId = request.SalesRegionContactRequest.AddrCountryId;
            SalesRegionContactInDb.GeoLat = request.SalesRegionContactRequest.GeoLat;
            SalesRegionContactInDb.GeoLong = request.SalesRegionContactRequest.GeoLong;
            SalesRegionContactInDb.UpdatedBy = request.SalesRegionContactRequest.UpdatedBy;

            var updateSalesRegionContactId = await SalesRegionContactService.UpdateSalesRegionContactAsync(SalesRegionContactInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSalesRegionContactId, message: "Record updated successfully.");
        }
    }
}
