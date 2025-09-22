namespace Application.Features.Distributors.Commands
{
    public class DeleteDistributorCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid DistributorId { get; set; }
    }

    public class DeleteDistributorCommandHandler(IDistributorService distributorService, IRegionService regionService) : IRequestHandler<DeleteDistributorCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteDistributorCommand request, CancellationToken cancellationToken)
        {
            var regions = await regionService.GetRegionsByDistributorAsync(request.DistributorId);
            if (regions.Count == 0)
            {
                var isDeleted = await distributorService.DeleteDistributorAsync(request.DistributorId);

                return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "Distributor deleted successfully.");
            }
            return await ResponseWrapper<bool>.SuccessAsync(data: false, message: "Regions exists for this Distributor and cannot be deleted.");
        }
    }
}
