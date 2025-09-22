namespace Application.Features.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid CustomerId { get; set; }
    }

    public class DeleteCustomerCommandHandler(ICustomerService CustomerService, ISiteService siteService) : IRequestHandler<DeleteCustomerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var sites = await siteService.GetSitesAsync(request.CustomerId);
            if (sites.Count == 0)
            {
                var deletedCustomer = await CustomerService.DeleteCustomerAsync(request.CustomerId);

                return await ResponseWrapper<bool>.SuccessAsync(data: deletedCustomer, message: "Customer deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "Sites exists for this Customer and cannot be deleted.");
        }
    }
}
