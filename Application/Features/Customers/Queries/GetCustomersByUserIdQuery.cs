using Application.Features.Customers.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers;

namespace Application.Features.Customers.Queries
{
    public class GetCustomersByUserIdQuery : IRequest<IResponseWrapper>
    {
        public Guid userId { get; set; }
    }

    public class GetCustomersByUserIdQueryHandler(ICustomerService CustomerService, ISiteService siteService) : IRequestHandler<GetCustomersByUserIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomersByUserIdQuery request, CancellationToken cancellationToken)
        {            
            var customerInDb = (await CustomerService.GetCustomersByUserIdAsync(request.userId)).Adapt<List<CustomerResponse>>();
            foreach (CustomerResponse customer in customerInDb)
            {
                customer.Sites = (await siteService.GetSitesbyUserIdAsync(customer.Id)).Adapt<List<SiteResponse>>();
            }


            if (customerInDb is not null)
            {
                return await ResponseWrapper<List<CustomerResponse>>.SuccessAsync(data: customerInDb);
            }
            return await ResponseWrapper<CustomerResponse>.SuccessAsync(message: "Customer does not exists.");
        }
    }
}
