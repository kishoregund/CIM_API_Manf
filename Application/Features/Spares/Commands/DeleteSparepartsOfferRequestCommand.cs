namespace Application.Features.Spares.Commands
{
    public class DeleteSparepartsOfferRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SparepartsOfferRequestId { get; set; }
    }

    public class DeleteSparepartsOfferRequestCommandHandler(ISparepartsOfferRequestService SparepartsOfferRequestService)
        : IRequestHandler<DeleteSparepartsOfferRequestCommand, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(DeleteSparepartsOfferRequestCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await SparepartsOfferRequestService.DeleteSparepartsOfferRequestAsync(request.SparepartsOfferRequestId);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Spareparts Offer Request deleted successfully.");
        }
    }
}