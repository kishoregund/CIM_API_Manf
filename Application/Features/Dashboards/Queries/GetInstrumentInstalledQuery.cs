using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;

namespace Application.Features.Dashboards.Queries
{
    public class GetInstrumentInstalledQuery : IRequest<IResponseWrapper>
    {
        public DashboardDateRequest DashboardDateRequest { get; set; }
    }

    public class GetInstrumentInstalledQueryHandler(IDistributorDashboardService distributorDashboardService) : IRequestHandler<GetInstrumentInstalledQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetInstrumentInstalledQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = await distributorDashboardService.GetInstrumentInstalled(request.DashboardDateRequest);

            if (brandInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}