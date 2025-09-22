using Application.Features.Identity.Users;

namespace Application.Features.Customers.Commands
{
    public class DeleteSiteContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SiteContactId { get; set; }
    }

    public class DeleteSiteContactCommandHandler(ISiteContactService SiteContactService, IUserService userService) : IRequestHandler<DeleteSiteContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSiteContactCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByContactIdAsync(request.SiteContactId, "CS", cancellationToken);
            if (user == null)
            {
                var deletedSiteContact = await SiteContactService.DeleteSiteContactAsync(request.SiteContactId);

                return await ResponseWrapper<bool>.SuccessAsync(data: deletedSiteContact, message: "Site Contact deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "This Contact is a user and can not be deleted.");
        }
    }
}
