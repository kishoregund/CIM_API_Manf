using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSitesQuery : IRequest<IResponseWrapper>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetSitesQueryHandler(ISiteService SitesService) : IRequestHandler<GetSitesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSitesQuery request, CancellationToken cancellationToken)
        {
            var SitesInDb = await SitesService.GetSitesAsync(request.CustomerId);

            if (SitesInDb.Count > 0)
            {
                return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(data: SitesInDb.Adapt<List<SiteResponse>>());
            }
            return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(message: "No Sites were found.");
        }
    }
}
