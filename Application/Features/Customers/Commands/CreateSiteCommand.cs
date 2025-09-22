
using Application.Features.Customers;
using Application.Features.Customers.Requests;

namespace Application.Features.Customers.Commands
{
    public class CreateSiteCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SiteRequest SiteRequest { get; set; }
    }

    public class CreateSiteCommandHandler(ISiteService SiteService) : IRequestHandler<CreateSiteCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSite = request.SiteRequest.Adapt<Site>();

            var SiteId = await SiteService.CreateSiteAsync(newSite);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SiteId, message: "Record saved successfully.");
        }
    }
}

