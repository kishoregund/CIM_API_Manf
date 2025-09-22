
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserProfiles.Commands
{
    public class DeleteUserProfilesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid UserProfilesId { get; set; }
    }

    public class DeleteUserProfilesCommandHandler(IUserProfilesService UserProfilesService) : IRequestHandler<DeleteUserProfilesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteUserProfilesCommand request, CancellationToken cancellationToken)
        {
            var deletedUserProfiles = await UserProfilesService.DeleteUserProfilesAsync(request.UserProfilesId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedUserProfiles, message: "UserProfiles deleted successfully.");
        }
    }
}
