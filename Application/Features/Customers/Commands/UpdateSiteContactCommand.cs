using Application.Features.Customers.Requests;
using Domain.Entities;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Application.Features.Customers.Commands
{


    public class UpdateSiteContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SiteContactRequest SiteContactRequest { get; set; }
    }

    public class UpdateSiteContactCommandHandler(ISiteContactService SiteContactService) : IRequestHandler<UpdateSiteContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSiteContactCommand request, CancellationToken cancellationToken)
        {
            var SiteContactInDb = await SiteContactService.GetSiteContactAsync(request.SiteContactRequest.Id);


            SiteContactInDb.Id = request.SiteContactRequest.Id;
            SiteContactInDb.DesignationId = request.SiteContactRequest.DesignationId;
            SiteContactInDb.Zip = request.SiteContactRequest.Zip;
            SiteContactInDb.SiteId = request.SiteContactRequest.SiteId;
            SiteContactInDb.FirstName = request.SiteContactRequest.FirstName;
            SiteContactInDb.LastName = request.SiteContactRequest.LastName;
            SiteContactInDb.MiddleName = request.SiteContactRequest.MiddleName;
            SiteContactInDb.PrimaryContactNo = request.SiteContactRequest.PrimaryContactNo;
            SiteContactInDb.PrimaryEmail = request.SiteContactRequest.PrimaryEmail;
            SiteContactInDb.SecondaryContactNo = request.SiteContactRequest.SecondaryContactNo;
            SiteContactInDb.SecondaryEmail = request.SiteContactRequest.SecondaryEmail;
            SiteContactInDb.WhatsappNo = request.SiteContactRequest.WhatsappNo;
            SiteContactInDb.Street = request.SiteContactRequest.Street;
            SiteContactInDb.Place = request.SiteContactRequest.Place;
            SiteContactInDb.Area = request.SiteContactRequest.Area;
            SiteContactInDb.City = request.SiteContactRequest.City;
            SiteContactInDb.AddrCountryId = request.SiteContactRequest.AddrCountryId;
            SiteContactInDb.GeoLat = request.SiteContactRequest.GeoLat;
            SiteContactInDb.GeoLong = request.SiteContactRequest.GeoLong;
            SiteContactInDb.UpdatedBy = request.SiteContactRequest.UpdatedBy;

            var updateSiteContactId = await SiteContactService.UpdateSiteContactAsync(SiteContactInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSiteContactId, message: "Record updated successfully.");
        }
    }
}
