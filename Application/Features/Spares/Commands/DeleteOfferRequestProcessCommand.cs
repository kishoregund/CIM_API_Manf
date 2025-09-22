namespace Application.Features.Spares.Commands
{
    public class DeleteOfferRequestProcessCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid OfferRequestProcessId { get; set; }
    }

    public class DeleteOfferRequestProcessCommandHandler(IOfferRequestProcessService OfferRequestProcessService)
        : IRequestHandler<DeleteOfferRequestProcessCommand, IResponseWrapper>
    {

        public async Task<IResponseWrapper> Handle(DeleteOfferRequestProcessCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await OfferRequestProcessService.DeleteOfferRequestProcessAsync(request.OfferRequestProcessId);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "Offer Request Process deleted successfully.");
        }
    }
}