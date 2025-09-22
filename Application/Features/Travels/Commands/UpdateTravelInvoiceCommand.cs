using Application.Features.AppBasic.Requests;
using Application.Features.Travels.Requests;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class UpdateTravelInvoiceCommand : IRequest<IResponseWrapper>
    {
        public UpdateTravelInvoiceRequest Request { get; set; }
    }

    public class UpdateTravelInvoiceCommandHandler(ITravelInvoiceService TravelInvoiceService)
        : IRequestHandler<UpdateTravelInvoiceCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateTravelInvoiceCommand request, CancellationToken cancellationToken)
        {
            var TravelInvoice = await TravelInvoiceService.GetTravelInvoiceEntityAsync(request.Request.Id);

            TravelInvoice.AmountBuild = request.Request.AmountBuild;
            TravelInvoice.DistributorId = request.Request.DistributorId;
            TravelInvoice.EngineerId = request.Request.EngineerId;
            TravelInvoice.ServiceRequestId = request.Request.ServiceRequestId;

            var result = await TravelInvoiceService.UpdateTravelInvoiceAsync(TravelInvoice);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}