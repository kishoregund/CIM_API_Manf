using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSiteContactsByCustomerQuery : IRequest<IResponseWrapper>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetSiteContactsByCustomerQueryHandler(ISiteContactService SiteContactsService) : IRequestHandler<GetSiteContactsByCustomerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteContactsByCustomerQuery request, CancellationToken cancellationToken)
        {
            var SiteContactsInDb = await SiteContactsService.GetSiteContactsByCustomerAsync(request.CustomerId);

            if (SiteContactsInDb.Count > 0)
            {
                return await ResponseWrapper<List<SiteContactResponse>>.SuccessAsync(data: SiteContactsInDb.Adapt<List<SiteContactResponse>>());
            }
            return await ResponseWrapper<SiteContactResponse>.SuccessAsync(message: "No Site Contacts were found.");
        }
    }
}
