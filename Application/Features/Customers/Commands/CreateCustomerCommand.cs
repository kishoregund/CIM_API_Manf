using Application.Features.Customers.Requests;
using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerRequest CustomerRequest { get; set; }
    }

    public class CreateCustomerCommandHandler(ICustomerService CustomerService) : IRequestHandler<CreateCustomerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // map

            var newCustomer = request.CustomerRequest.Adapt<Customer>();

            var CustomerId = await CustomerService.CreateCustomerAsync(newCustomer);

            return await ResponseWrapper<Guid>.SuccessAsync(data: CustomerId, message: "Record saved successfully.");
        }
    }
}

