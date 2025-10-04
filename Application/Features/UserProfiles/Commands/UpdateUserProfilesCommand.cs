
using Domain.Entities;
using Application.Features.UserProfiles.Requests;

namespace Application.Features.UserProfiles.Commands
{
    public class UpdateUserProfilesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UserProfilesRequest UserProfilesRequest { get; set; }
    }

    public class UpdateUserProfilesCommandHandler(IUserProfilesService UserProfilesService) : IRequestHandler<UpdateUserProfilesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateUserProfilesCommand request, CancellationToken cancellationToken)
        {
            var UserProfilesInDb = await UserProfilesService.GetUserProfileEntityAsync(request.UserProfilesRequest.Id);

            UserProfilesInDb.Id = request.UserProfilesRequest.Id;
            UserProfilesInDb.CustSites = request.UserProfilesRequest.CustSites;
            UserProfilesInDb.BrandIds = request.UserProfilesRequest.BrandIds;
            UserProfilesInDb.BusinessUnitIds = request.UserProfilesRequest.BusinessUnitIds;
            UserProfilesInDb.DistRegions = request.UserProfilesRequest.DistRegions;
            UserProfilesInDb.Description = request.UserProfilesRequest.Description;
            UserProfilesInDb.SegmentId = request.UserProfilesRequest.SegmentId;
            UserProfilesInDb.RoleId = request.UserProfilesRequest.RoleId;
            UserProfilesInDb.UserId = request.UserProfilesRequest.UserId;
            UserProfilesInDb.ProfileFor = request.UserProfilesRequest.ProfileFor;
            UserProfilesInDb.ManfBUIds = request.UserProfilesRequest.ManfBUIds;
            UserProfilesInDb.UpdatedBy = request.UserProfilesRequest.UpdatedBy;

            var updateUserProfilesId = await UserProfilesService.UpdateUserProfilesAsync(UserProfilesInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateUserProfilesId, message: "Record updated successfully.");
        }
    }
}
