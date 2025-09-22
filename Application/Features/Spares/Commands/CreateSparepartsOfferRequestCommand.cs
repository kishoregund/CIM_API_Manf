using Application.Features.Spares.Requests;

namespace Application.Features.Spares.Commands
{
    public class CreateSparepartsOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public List<SparepartOfferRequestRequest> SparepartsOfferRequestRequest { get; set; }
    }

    public class CreateSparepartsOfferRequestCommandHandler(ISparepartsOfferRequestService SparepartsOfferRequestService)
        : IRequestHandler<CreateSparepartsOfferRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSparepartsOfferRequestCommand request, CancellationToken cancellationToken)
        {
            // map

            //var newSparepartsOfferRequest = request.SparepartsOfferRequestRequest.Adapt<SparepartsOfferRequest>();

            var SparepartsOfferRequestId = await SparepartsOfferRequestService.CreateSparepartsOfferRequestAsync(request.SparepartsOfferRequestRequest);

            return await ResponseWrapper<bool>.SuccessAsync(data: SparepartsOfferRequestId, message: "Spareparts Offer Request created successfully.");
        }
    }
}