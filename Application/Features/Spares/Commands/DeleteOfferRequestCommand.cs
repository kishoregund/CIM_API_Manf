namespace Application.Features.Spares.Commands
{
    public class DeleteOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid OfferRequestId { get; set; }
    }

    public class DeleteOfferRequestCommandHandler(IOfferRequestService OfferRequestService)
        : IRequestHandler<DeleteOfferRequestCommand, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(DeleteOfferRequestCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await OfferRequestService.DeleteOfferRequestAsync(request.OfferRequestId);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Offer Request deleted successfully.");
        }
    }
}