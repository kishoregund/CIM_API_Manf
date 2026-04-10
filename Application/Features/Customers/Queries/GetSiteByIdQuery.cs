using Application.Features.Customers.Responses;
using Application.Features.Masters;

namespace Application.Features.Customers.Queries
{
    public class GetSiteByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid SiteId { get; set; }
    }

    public class GetSiteByIdQueryHandler(ISiteService SiteService, ISiteContactService siteContactService, ICountryService countryService) : IRequestHandler<GetSiteByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteByIdQuery request, CancellationToken cancellationToken)
        {
            var SiteInDb = (await SiteService.GetSiteAsync(request.SiteId)).Adapt<SiteResponse>();
            SiteInDb.CountryName = countryService.GetCountryAsync(SiteInDb.CountryId).Result.Name;
            SiteInDb.SiteContacts = await siteContactService.GetSiteContactsAsync(request.SiteId);

            if (SiteInDb is not null)
            {
                return await ResponseWrapper<SiteResponse>.SuccessAsync(data: SiteInDb);
            }
            return await ResponseWrapper<SiteResponse>.SuccessAsync(message: "Site does not exists.");
        }
    }
}
