using Application.Features.AppBasic.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class  GetSiteInstrumentQuery : IRequest<IResponseWrapper>
    {
        public string Id { get; set; }
    }

    public class  GetSiteInstrumentQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler< GetSiteInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle( GetSiteInstrumentQuery request, CancellationToken cancellationToken)
        {
            var instrumentInDb = await CustomerDashboardService. GetSiteInstrumentAsync(request.Id);

            if (instrumentInDb is not null)
            {
                return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(data: instrumentInDb);
            }
            return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}