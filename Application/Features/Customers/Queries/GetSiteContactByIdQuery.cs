using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetSiteContactByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid SiteContactId { get; set; }
    }

    public class GetSiteContactByIdQueryHandler(ISiteContactService SiteContactService) : IRequestHandler<GetSiteContactByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteContactByIdQuery request, CancellationToken cancellationToken)
        {
            var SiteContactInDb = (await SiteContactService.GetSiteContactAsync(request.SiteContactId)).Adapt<SiteContactResponse>();

            if (SiteContactInDb is not null)
            {
                return await ResponseWrapper<SiteContactResponse>.SuccessAsync(data: SiteContactInDb);
            }
            return await ResponseWrapper<SiteContactResponse>.SuccessAsync(message: "SiteContact does not exists.");
        }
    }
}
