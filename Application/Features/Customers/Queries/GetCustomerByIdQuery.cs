using Application.Features.Customers.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers;

namespace Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetCustomerByIdQueryHandler(ICustomerService CustomerService, ISiteService siteService) : IRequestHandler<GetCustomerByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customerInDb = (await CustomerService.GetCustomerAsync(request.CustomerId)).Adapt<CustomerResponse>();
            customerInDb.Sites = (await siteService.GetSitesbyUserIdAsync(request.CustomerId)).Adapt<List<SiteResponse>>(); 

            if (customerInDb is not null)
            {
                return await ResponseWrapper<CustomerResponse>.SuccessAsync(data: customerInDb);
            }
            return await ResponseWrapper<CustomerResponse>.SuccessAsync(message: "Customer does not exists.");
        }
    }
}
