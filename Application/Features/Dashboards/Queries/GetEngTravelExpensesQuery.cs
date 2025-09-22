using Application.Features.AppBasic.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetEngTravelExpensesQuery : IRequest<IResponseWrapper>
    {
        public string Date{ get; set; }
    }

    public class GetEngTravelExpensesQueryHandler(IEngineerDashboardService EngineerDashboardService) : IRequestHandler<GetEngTravelExpensesQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngTravelExpensesQuery request, CancellationToken cancellationToken)
        {
            var srInDb = await EngineerDashboardService.GetTravelExpensesAsync(request.Date);

            if (srInDb is not null)
            {
                return await ResponseWrapper<object>.SuccessAsync(data: srInDb);
            }
            return await ResponseWrapper<object>.SuccessAsync(message: "Data does not exists.");
        }
    }
}