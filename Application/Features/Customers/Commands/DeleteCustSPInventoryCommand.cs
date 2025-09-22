using Application.Features.Customers;

namespace Application.Features.Customers.Commands
{
    public class DeleteCustSPInventoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid CustSPInventoryId { get; set; }
    }

    public class DeleteCustSPInventoryCommandHandler(ICustSPInventoryService CustSPInventoryService) : IRequestHandler<DeleteCustSPInventoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCustSPInventoryCommand request, CancellationToken cancellationToken)
        {
            var deletedCustSPInventory = await CustSPInventoryService.DeleteCustSPInventoryAsync(request.CustSPInventoryId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedCustSPInventory, message: "CustSPInventory deleted successfully.");
        }
    }
}
