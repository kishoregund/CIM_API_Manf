using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetCustomersQueryHandler(ICustomerService CustomerService, ISiteService siteService) : IRequestHandler<GetCustomersQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var CustomersInDb = (await CustomerService.GetCustomersAsync()).Adapt<List<CustomerResponse>>();

            foreach (var Customer in CustomersInDb)
            {
                Customer.Sites = (await siteService.GetSitesAsync(Customer.Id)).Adapt<List<SiteResponse>>();
            }
            if (CustomersInDb.Count > 0)
            {
                return await ResponseWrapper<List<CustomerResponse>>.SuccessAsync(data: CustomersInDb.Adapt<List<CustomerResponse>>());
            }
            return await ResponseWrapper<CustomerResponse>.SuccessAsync(message: "No Customers were found.");
        }
    }
}
