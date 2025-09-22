using Application.Features.Distributors;
using Application.Features.Distributors.Responses;

namespace Application.Features.UserProfiles.Queries
{
    public class GetRegionsByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetRegionsByContactIdQueryHandler(IUserProfilesService userProfilesService ) : IRequestHandler<GetRegionsByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRegionsByContactIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await userProfilesService.GetRegionsByContactIdAsync(request.ContactId)).Adapt<List<RegionResponse>>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(message: "Regions does not exists.");
        }
    }
}