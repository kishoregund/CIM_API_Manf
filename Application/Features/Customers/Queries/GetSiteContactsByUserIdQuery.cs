using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSiteContactsByUserIdQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetSiteContactsByUserIdQueryHandler(ISiteContactService SiteContactsService) : IRequestHandler<GetSiteContactsByUserIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteContactsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var SiteContactsInDb = await SiteContactsService.GetSiteContactsByUserIdAsync();

            if (SiteContactsInDb.Count > 0)
            {
                return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(data: SiteContactsInDb.Adapt<List<CustomerInstrumentResponse>>());
            }
            return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(message: "No SiteContacts were found.");
        }
    }
}
