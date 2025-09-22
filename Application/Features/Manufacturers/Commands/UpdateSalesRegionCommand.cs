
using System.Globalization;
using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class UpdateSalesRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SalesRegionRequest SalesRegionRequest { get; set; }
    }

    public class UpdateSalesSalesRegionCommandHandler(ISalesRegionService SalesRegionService) : IRequestHandler<UpdateSalesRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateSalesRegionCommand request, CancellationToken cancellationToken)
        {
            var SalesRegionInDb = await SalesRegionService.GetSalesRegionAsync(request.SalesRegionRequest.Id);

           
            SalesRegionInDb.SalesRegionName = request.SalesRegionRequest.SalesRegionName;
            SalesRegionInDb.ManfId = request.SalesRegionRequest.ManfId;
            SalesRegionInDb.IsBlocked = request.SalesRegionRequest.IsBlocked;
            SalesRegionInDb.IsPrincipal = request.SalesRegionRequest.IsPrincipal;
            SalesRegionInDb.PayTerms = request.SalesRegionRequest.PayTerms;
            SalesRegionInDb.Countries = request.SalesRegionRequest.Countries;

            SalesRegionInDb.IsActive = request.SalesRegionRequest.IsActive;
            SalesRegionInDb.UpdatedBy = Guid.Empty;
            SalesRegionInDb.UpdatedOn = DateTime.Now;
            SalesRegionInDb.Area = request.SalesRegionRequest.Area;
            SalesRegionInDb.City = request.SalesRegionRequest.City;
            SalesRegionInDb.AddrCountryId = request.SalesRegionRequest.AddrCountryId;
            SalesRegionInDb.GeoLat = request.SalesRegionRequest.GeoLat;
            SalesRegionInDb.GeoLong = request.SalesRegionRequest.GeoLong;
            SalesRegionInDb.Place = request.SalesRegionRequest.Place;
            SalesRegionInDb.Street = request.SalesRegionRequest.Street;
            SalesRegionInDb.Zip = request.SalesRegionRequest.Zip;

            var updateSalesRegionId = await SalesRegionService.UpdateSalesRegionAsync(SalesRegionInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateSalesRegionId, message: "Record updated successfully.");
        }
    }
}
