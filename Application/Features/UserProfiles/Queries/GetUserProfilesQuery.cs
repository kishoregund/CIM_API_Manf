
using Application.Features.UserProfiles.Responses;
using Domain.Views;

namespace Application.Features.UserProfiles.Queries
{
    public class GetUserProfilesQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetUserProfilesQueryHandler(IUserProfilesService UserProfilesService) : IRequestHandler<GetUserProfilesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserProfilesQuery request, CancellationToken cancellationToken)
        {
            var UserProfilesInDb = await UserProfilesService.GetUserProfilesAsync();

            if (UserProfilesInDb.Count > 0)
            {
                return await ResponseWrapper<List<VW_UserProfile>>.SuccessAsync(data: UserProfilesInDb.Adapt<List<VW_UserProfile>>());
            }
            return await ResponseWrapper<VW_UserProfile>.SuccessAsync(message: "No User Profiles were found.");
        }
    }
}
