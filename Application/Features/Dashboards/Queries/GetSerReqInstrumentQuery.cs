using Application.Features.AppBasic.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class  GetSerReqInstrumentQuery : IRequest<IResponseWrapper>
    {
        public string Id { get; set; }
    }

    public class  GetSerReqInstrumentQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler< GetSerReqInstrumentQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle( GetSerReqInstrumentQuery request, CancellationToken cancellationToken)
        {
            var instrumentInDb = await CustomerDashboardService. GetSerReqInstrumentAsync(request.Id);

            if (instrumentInDb is not null)
            {
                return await ResponseWrapper<List<CustomerInstrumentResponse>>.SuccessAsync(data: instrumentInDb);
            }
            return await ResponseWrapper<CustomerInstrumentResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}