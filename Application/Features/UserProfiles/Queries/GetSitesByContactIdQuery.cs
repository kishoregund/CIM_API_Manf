using Application.Features.Customers.Responses;

namespace Application.Features.UserProfiles.Queries
{
    public class GetSitesByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetSitesByContactIdQueryHandler(IUserProfilesService userProfilesService ) : IRequestHandler<GetSitesByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSitesByContactIdQuery request, CancellationToken cancellationToken)
        {
            var SiteContactInDb = (await userProfilesService.GetSitesByContactIdAsync(request.ContactId)).Adapt<List<SiteResponse>>();

            if (SiteContactInDb is not null)
            {
                return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(data: SiteContactInDb);
            }
            return await ResponseWrapper<List<SiteResponse>>.SuccessAsync(message: "Sites does not exists.");
        }
    }
}