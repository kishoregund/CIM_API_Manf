using Application.Features.Manufacturers.Commands;
using Application.Features.Manufacturers;

namespace Application.Features.Manufacturers.Commands
{
    public class DeleteManufacturerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid ManufacturerId { get; set; }
    }

    public class DeleteManufacturerCommandHandler(IManufacturerService ManufacturerService) : IRequestHandler<DeleteManufacturerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
        {
            var deletedManufacturer = await ManufacturerService.DeleteManufacturerAsync(request.ManufacturerId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedManufacturer, message: "Manufacturer deleted successfully.");
        }
    }
}
