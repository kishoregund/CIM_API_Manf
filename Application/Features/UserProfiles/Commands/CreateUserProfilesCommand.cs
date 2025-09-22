
using Application.Features.UserProfiles.Requests;

namespace Application.Features.UserProfiles.Commands
{
    public class CreateUserProfilesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UserProfilesRequest UserProfilesRequest { get; set; }
    }

    public class CreateUserProfilesCommandHandler(IUserProfilesService UserProfilesService) : IRequestHandler<CreateUserProfilesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateUserProfilesCommand request, CancellationToken cancellationToken)
        {
            // map

            var newUserProfiles = request.UserProfilesRequest.Adapt<Domain.Entities.UserProfiles>();

            var UserProfilesId = await UserProfilesService.CreateUserProfilesAsync(newUserProfiles);

            return await ResponseWrapper<Guid>.SuccessAsync(data: UserProfilesId, message: "Record saved successfully.");
        }
    }
}
