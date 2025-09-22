
using Application.Features.Distributors.Requests;
using Domain.Entities;


namespace Application.Features.Distributors.Commands
{
    public class CreateRegionCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public RegionRequest RegionRequest { get; set; }
    }

    public class CreateRegionCommandHandler(IRegionService regionService)
        : IRequestHandler<CreateRegionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = request.RegionRequest.Adapt<Regions>();

            var regionId = await regionService.CreateRegionAsync(region);

            return await ResponseWrapper<Guid>.SuccessAsync(data: regionId, message: "Record saved successfully.");

        }
    }
}