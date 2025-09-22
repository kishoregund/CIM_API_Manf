using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class CreateOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public OfferRequestRequest OfferRequestRequest { get; set; }
    }

    public class CreateOfferRequestCommandHandler(IOfferRequestService OfferRequestService)
        : IRequestHandler<CreateOfferRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateOfferRequestCommand request, CancellationToken cancellationToken)
        {
            // map

            var newOfferRequest = request.OfferRequestRequest.Adapt<OfferRequest>();
            newOfferRequest.Instruments = request.OfferRequestRequest.InstrumentsList;
            var OfferRequestId = await OfferRequestService.CreateOfferRequestAsync(newOfferRequest);

            return await ResponseWrapper<Guid>.SuccessAsync(data: OfferRequestId,
                message: "Record saved successfully.");
        }
    }
}