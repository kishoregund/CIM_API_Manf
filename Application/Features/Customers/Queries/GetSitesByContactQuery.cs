using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSitesByContactQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }

    public class GetSitesByContactQueryHandler(ISiteService SitesService) : IRequestHandler<GetSitesByContactQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSitesByContactQuery request, CancellationToken cancellationToken)
        {
            var SitesInDb = await SitesService.GetSitesByContactAsync(request.ContactId);

            if (SitesInDb.Count > 0)
            {
                return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(data: SitesInDb.Adapt<List<SiteResponse>>());
            }
            return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(message: "No Sites were found.");
        }
    }
}
