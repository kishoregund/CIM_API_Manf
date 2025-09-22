using Application.Features.Manufacturers;

namespace Application.Features.Manufacturers.Commands
{
    public class DeleteSalesRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SalesRegionId { get; set; }
    }

    public class DeleteSalesRegionCommandHandler(ISalesRegionService SalesRegionService) : IRequestHandler<DeleteSalesRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSalesRegionCommand request, CancellationToken cancellationToken)
        {
            var deletedSalesRegion = await SalesRegionService.DeleteSalesRegionAsync(request.SalesRegionId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSalesRegion, message: "SalesRegion deleted successfully.");
        }
    }
}
