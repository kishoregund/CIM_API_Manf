
using Domain.Entities;
using System.IO;
using Application.Features.Customers.Requests;

namespace Application.Features.Customers.Commands
{
    public class UpdateSiteCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SiteRequest SiteRequest { get; set; }
    }

    public class UpdateSiteCommandHandler(ISiteService SiteService) : IRequestHandler<UpdateSiteCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
        {
            var SiteInDb = await SiteService.GetSiteAsync(request.SiteRequest.Id);


            SiteInDb.CustomerId = request.SiteRequest.CustomerId;
            SiteInDb.CustRegName = request.SiteRequest.CustRegName;
            SiteInDb.DistId = request.SiteRequest.DistId;
            SiteInDb.RegionId = request.SiteRequest.RegionId;
            SiteInDb.IsBlocked = request.SiteRequest.IsBlocked;
            SiteInDb.IsActive = request.SiteRequest.IsActive;
            SiteInDb.PayTerms = request.SiteRequest.PayTerms;
            SiteInDb.RegName = request.SiteRequest.RegName;
            SiteInDb.Street = request.SiteRequest.Street;
            SiteInDb.Place = request.SiteRequest.Place;
            SiteInDb.Area = request.SiteRequest.Area;
            SiteInDb.City = request.SiteRequest.City;
            SiteInDb.CountryId = request.SiteRequest.CountryId;
            SiteInDb.GeoLat = request.SiteRequest.GeoLat;
            SiteInDb.GeoLong = request.SiteRequest.GeoLong;
            SiteInDb.UpdatedBy = request.SiteRequest.UpdatedBy;

            var updateSiteId = await SiteService.UpdateSiteAsync(SiteInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSiteId, message: "Record updated successfully.");
        }
    }
}
