using Application.Features.Dashboards;
using Application.Features.Customers.Responses;

namespace Application.Features.Dashboards.Queries
{
    public class GetCustomerDetailsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetCustomerDetailsQueryHandler(ICustomerDashboardService CustomerDashboardService) : IRequestHandler<GetCustomerDetailsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            var customerInDb = await CustomerDashboardService.GetCustomerDetailsAsync();

            if (customerInDb is not null)
            {
                return await ResponseWrapper<CustomerResponse>.SuccessAsync(data: customerInDb);
            }
            return await ResponseWrapper<CustomerResponse>.SuccessAsync(message: "Data does not exists.");
        }
    }
}