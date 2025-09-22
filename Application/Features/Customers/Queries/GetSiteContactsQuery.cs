using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSiteContactsQuery : IRequest<IResponseWrapper>
    {
        public Guid SiteId { get; set; }
    }

    public class GetSiteContactsQueryHandler(ISiteContactService SiteContactsService) : IRequestHandler<GetSiteContactsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteContactsQuery request, CancellationToken cancellationToken)
        {
            var SiteContactsInDb = await SiteContactsService.GetSiteContactsAsync(request.SiteId);

            if (SiteContactsInDb.Count > 0)
            {
                return await ResponseWrapper<List<SiteContactResponse>>.SuccessAsync(data: SiteContactsInDb.Adapt<List<SiteContactResponse>>());
            }
            return await ResponseWrapper<List<SiteContactResponse>>.SuccessAsync(message: "No SiteContacts were found.");
        }
    }
}
