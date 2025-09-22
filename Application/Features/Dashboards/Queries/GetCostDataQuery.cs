using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;

namespace Application.Features.Dashboards.Queries
{
    public class GetCostDataQuery : IRequest<IResponseWrapper>
    {
        public DashboardDateRequest DashboardDateRequest { get; set; }
    }

    public class GetCostDataQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetCostDataQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCostDataQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = await CustomerDashboardService.GetCostDataAsync(request.DashboardDateRequest);

            if (brandInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}