using Application.Features.Customers;
using Application.Features.Customers.Requests;


namespace Application.Features.Customers.Commands
{
    public class CreateSiteContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SiteContactRequest SiteContactRequest { get; set; }
    }

    public class CreateSiteContactCommandHandler(ISiteContactService SiteContactService) : IRequestHandler<CreateSiteContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSiteContactCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSiteContact = request.SiteContactRequest.Adapt<SiteContact>();

            var SiteContactId = await SiteContactService.CreateSiteContactAsync(newSiteContact);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SiteContactId, message: "Record saved successfully.");
        }
    }
}


