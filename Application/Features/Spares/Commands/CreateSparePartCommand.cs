using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class CreateSparepartCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SparepartRequest SparepartRequest { get; set; }
    }

    public class CreateSparepartCommandHandler(ISparepartService sparePartService)
        : IRequestHandler<CreateSparepartCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSparepartCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSparepart = request.SparepartRequest.Adapt<Sparepart>();

            var sparepartId = await sparePartService.CreateSparepartAsync(newSparepart);

            return await ResponseWrapper<Guid>.SuccessAsync(data: sparepartId,
                message: "Record saved successfully.");
        }
    }
}