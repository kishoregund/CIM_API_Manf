using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.Customers.Commands
{
    public class DeleteSiteCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SiteId { get; set; }
    }

    public class DeleteSiteCommandHandler(ISiteService SiteService, ISiteContactService siteContactService) : IRequestHandler<DeleteSiteCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var siteContacts = await siteContactService.GetSiteContactsAsync(request.SiteId);
            if (siteContacts.Count == 0)
            {
                var deletedSite = await SiteService.DeleteSiteAsync(request.SiteId);

                return await ResponseWrapper<bool>.SuccessAsync(data: deletedSite, message: "Site deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "Contacts exists for this Site and cannot be deleted.");
        }
    }
}
