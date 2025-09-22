
using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class CreateManufacturerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ManufacturerRequest ManufacturerRequest { get; set; }
    }

    public class CreateManufacturerCommandHandler(IManufacturerService ManufacturerService) : IRequestHandler<CreateManufacturerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
        {
            // map

            var newManufacturer = request.ManufacturerRequest.Adapt<Manufacturer>();

            var ManufacturerId = await ManufacturerService.CreateManufacturerAsync(newManufacturer);

            return await ResponseWrapper<Guid>.SuccessAsync(data: ManufacturerId, message: "Record saved successfully.");
        }
    }
}
