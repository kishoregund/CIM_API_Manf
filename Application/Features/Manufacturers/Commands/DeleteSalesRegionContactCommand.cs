using Application.Features.Manufacturers;

namespace Application.Features.Manufacturers.Commands
{
    public class DeleteSalesRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SalesRegionContactId { get; set; }
    }

    public class DeleteSalesRegionContactCommandHandler(ISalesRegionContactService SalesRegionContactService) : IRequestHandler<DeleteSalesRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSalesRegionContactCommand request, CancellationToken cancellationToken)
        {
            var deletedSalesRegionContact = await SalesRegionContactService.DeleteSalesRegionContactAsync(request.SalesRegionContactId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSalesRegionContact, message: "SalesRegionContact deleted successfully.");
        }
    }
}
