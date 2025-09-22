
using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.ServiceRequests.Responses;
using Application.Features.Spares.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetAllOfferrequestQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllOfferrequestQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetAllOfferrequestQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllOfferrequestQuery request, CancellationToken cancellationToken)
        {
            var offerInDb = await CustomerDashboardService.GetAllOfferrequestAsync();

            if (offerInDb is not null)
            {
                return await ResponseWrapper<List<OfferRequestResponse>>.SuccessAsync(data: offerInDb);
            }
            return await ResponseWrapper<OfferRequestResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}