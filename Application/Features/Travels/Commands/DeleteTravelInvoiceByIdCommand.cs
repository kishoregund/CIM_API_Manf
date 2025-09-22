
namespace Application.Features.Travels.Commands
{
    public class DeleteTravelInvoiceByIdCommand : IRequest<IResponseWrapper>
    {
        public Guid Id { get; set; }
    }
    public class DeleteTravelInvoiceByIdCommandHandler(ITravelInvoiceService TravelInvoiceService)
        : IRequestHandler<DeleteTravelInvoiceByIdCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteTravelInvoiceByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await TravelInvoiceService.DeleteTravelInvoiceAsync(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted,
                message: "TravelInvoice deleted successfully.");
        }
    }
}