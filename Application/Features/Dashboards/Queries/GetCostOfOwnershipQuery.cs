using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetCostOfOwnershipQuery : IRequest<IResponseWrapper>
    {
        public string Id { get; set; }
    }

    public class GetCostOfOwnershipQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetCostOfOwnershipQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCostOfOwnershipQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = await CustomerDashboardService.GetCostOfOwnerShipAsync(request.Id);

            if (brandInDb is not null)
            {
                return await ResponseWrapper<InstrumentOwnershipResponse>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<InstrumentOwnershipResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}