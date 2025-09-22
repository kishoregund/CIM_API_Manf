using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class CreateSalesRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SalesRegionRequest SalesRegionRequest { get; set; }
    }

    public class CreateSalesRegionCommandHandler(ISalesRegionService SalesRegionService) : IRequestHandler<CreateSalesRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSalesRegionCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSalesRegion = request.SalesRegionRequest.Adapt<SalesRegion>();

            var SalesRegionId = await SalesRegionService.CreateSalesRegionAsync(newSalesRegion);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SalesRegionId, message: "Record saved successfully.");
        }
    }
}
