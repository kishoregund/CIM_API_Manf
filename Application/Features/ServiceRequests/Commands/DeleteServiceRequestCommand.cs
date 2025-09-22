using Application.Features.ServiceRequests.Commands;
using Application.Features.ServiceRequests;

namespace Application.Features.ServiceRequests.Commands
{
    public class DeleteServiceRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class DeleteServiceRequestCommandHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<DeleteServiceRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteServiceRequestCommand request, CancellationToken cancellationToken)
        {
            var deletedServiceRequest = await ServiceRequestService.DeleteServiceRequestAsync(request.ServiceRequestId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedServiceRequest, message: "ServiceRequest deleted successfully.");
        }
    }
}
