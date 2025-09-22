
using Application.Features.Distributors.Requests;
using Domain.Entities;


namespace Application.Features.Distributors.Commands
{
    public class CreateRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public RegionContactRequest RegionContactRequest { get; set; }
    }

    public class CreateRegionContactCommandHandler(IRegionContactService regionContactService)
        : IRequestHandler<CreateRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateRegionContactCommand request, CancellationToken cancellationToken)
        {
            var region = request.RegionContactRequest.Adapt<RegionContact>();

            var regionId = await regionContactService.CreateRegionContactAsync(region);

            return await ResponseWrapper<Guid>.SuccessAsync(data: regionId, message: "Record saved successfully.");

        }
    }
}
