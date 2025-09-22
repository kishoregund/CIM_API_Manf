using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class CreateOfferRequestProcessCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public OfferRequestProcessRequest OfferRequestProcessRequest { get; set; }
    }

    public class CreateOfferRequestProcessCommandHandler(IOfferRequestProcessService OfferRequestProcessService)
        : IRequestHandler<CreateOfferRequestProcessCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateOfferRequestProcessCommand request, CancellationToken cancellationToken)
        {
            // map

            var newOfferRequestProcess = request.OfferRequestProcessRequest.Adapt<OfferRequestProcess>();

            var OfferRequestProcessId = await OfferRequestProcessService.CreateOfferRequestProcessAsync(newOfferRequestProcess);

            return await ResponseWrapper<Guid>.SuccessAsync(data: OfferRequestProcessId,
                message: "Record saved successfully.");
        }
    }
}