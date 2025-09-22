using Application.Features.Masters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Masters.Queries
{
    public class GetScreensForRolePermissionsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetScreensForRolePermissionsQueryHandler(IListTypeItemsService listTypeItemsService) : IRequestHandler<GetScreensForRolePermissionsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetScreensForRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var screensInDb = await listTypeItemsService.GetScreensForRolePermissions();
            if (screensInDb.Count > 0)
            {
                return await ResponseWrapper<List<ScreensForRolePermissionsResponse>>.SuccessAsync(data: screensInDb.Adapt<List<ScreensForRolePermissionsResponse>>());
            }
            return await ResponseWrapper<List<ScreensForRolePermissionsResponse>>.SuccessAsync(message: "No Screens were found.");
        }
    }
}