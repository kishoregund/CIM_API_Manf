
using Application.Features.Travels.Requests;

namespace Application.Features.Travels.Commands
{
    public class CreateTravelInvoiceCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public TravelInvoiceRequest Request { get; set; }
    }
    public class CreateTravelInvoiceCommandHandler(ITravelInvoiceService TravelInvoiceService)
        : IRequestHandler<CreateTravelInvoiceCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateTravelInvoiceCommand request, CancellationToken cancellationToken)
        {
            var TravelInvoice = request.Request.Adapt<TravelInvoice>();
            var result = await TravelInvoiceService.CreateTravelInvoiceAsync(TravelInvoice);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record saved successfully.");
        }
    }
}