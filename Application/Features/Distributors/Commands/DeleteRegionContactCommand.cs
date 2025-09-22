using Application.Features.Identity.Users;

namespace Application.Features.Distributors.Commands
{
    public class DeleteRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid RegionContactId { get; set; }
    }

    public class DeleteRegionContactCommandHandler(IRegionContactService regionContactService, IUserService userService) : IRequestHandler<DeleteRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteRegionContactCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByContactIdAsync(request.RegionContactId, "DR", cancellationToken);
            if (user == null)
            {
                var isDeleted = await regionContactService.DeleteRegionContactAsync(request.RegionContactId);

                return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "Contact deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "This Contact is a user and can not be deleted.");
        }
    }
}
