using Application.Features.AppBasic.Requests;
using Domain.Entities;

namespace Application.Features.Travels.Commands
{
    public class UpdateAdvanceRequestCommand : IRequest<IResponseWrapper>
    {
        public AdvanceRequest Request { get; set; }
    }

    public class UpdateAdvanceRequestCommandHandler(IAdvanceRequestService AdvanceRequestService)
        : IRequestHandler<UpdateAdvanceRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateAdvanceRequestCommand request, CancellationToken cancellationToken)
        {
            var AdvanceRequest = await AdvanceRequestService.GetAdvanceRequestEntityByIdAsync(request.Request.Id);

            AdvanceRequest.Id = request.Request.Id;
            AdvanceRequest.IsActive = request.Request.IsActive;

            AdvanceRequest.EngineerId = request.Request.EngineerId;
            AdvanceRequest.DistributorId = request.Request.DistributorId;
            AdvanceRequest.CustomerId = request.Request.CustomerId;
            AdvanceRequest.ServiceRequestId = request.Request.ServiceRequestId;
            AdvanceRequest.AdvanceAmount = request.Request.AdvanceAmount;
            AdvanceRequest.AdvanceCurrency = request.Request.AdvanceCurrency;
            AdvanceRequest.ClientNameLocation = request.Request.ClientNameLocation;
            AdvanceRequest.IsBillable = request.Request.IsBillable;
            AdvanceRequest.OfficeLocationId = request.Request.OfficeLocationId;
            AdvanceRequest.ReportingManager = request.Request.ReportingManager;
            AdvanceRequest.UnderTaking = request.Request.UnderTaking;

            var result = await AdvanceRequestService.UpdateAdvanceRequestAsync(AdvanceRequest);
            return await ResponseWrapper<Guid>.SuccessAsync(data: result, message: "Record updated successfully.");
        }
    }
}