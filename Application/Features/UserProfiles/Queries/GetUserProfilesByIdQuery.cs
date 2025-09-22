
using Application.Features.UserProfiles.Responses;
using Domain.Views;

namespace Application.Features.UserProfiles.Queries
{
    public class GetUserProfilesByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid UserProfilesId { get; set; }
    }

    public class GetUserProfilesByIdQueryHandler(IUserProfilesService UserProfilesService) : IRequestHandler<GetUserProfilesByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUserProfilesByIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = (await UserProfilesService.GetUserProfileAsync(request.UserProfilesId)).Adapt<VW_UserProfile>();

            if (userProfile is not null)
            {
                return await ResponseWrapper<VW_UserProfile>.SuccessAsync(data: userProfile);
            }
            return await ResponseWrapper<VW_UserProfile>.SuccessAsync(message: "User Profile does not exists.");
        }
    }
}
