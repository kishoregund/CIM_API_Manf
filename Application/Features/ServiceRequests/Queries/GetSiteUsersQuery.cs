using Application.Features.Customers;
using Application.Features.ServiceRequests.Responses;
using Application.Features.UserProfiles.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSiteUsersQuery : IRequest<IResponseWrapper>
    {
        public Guid SiteId { get; set; }
    }

    public class GetSiteUsersQueryHandler(ISiteService siteService) : IRequestHandler<GetSiteUsersQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSiteUsersQuery request, CancellationToken cancellationToken)
        {
            var siteUsersInDb = (await siteService.GetSiteUsersAsync(request.SiteId)).Adapt<List<UserByContactResponse>>();

            if (siteUsersInDb is not null)
            {
                return await ResponseWrapper<List<UserByContactResponse>>.SuccessAsync(data: siteUsersInDb);
            }
            return await ResponseWrapper<List<UserByContactResponse>>.SuccessAsync(message: "Site Users does not exists.");
        }
    }
}
