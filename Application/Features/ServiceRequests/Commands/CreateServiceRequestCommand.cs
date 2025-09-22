using Application.Features.Schools.Commands;
using Application.Features.Schools;
using Application.Features.ServiceRequests.Responses;
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateServiceRequestCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public ServiceRequestRequest ServiceRequestRequest { get; set; }
    }

    public class CreateServiceRequestCommandHandler(IServiceRequestService serviceRequestService) : IRequestHandler<CreateServiceRequestCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
        {
            // map

            var newServiceRequest = request.ServiceRequestRequest.Adapt<Domain.Entities.ServiceRequest>();

            var SRId = await serviceRequestService.CreateServiceRequestAsync(newServiceRequest);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SRId, message: "Record saved successfully.");
        }
    }
}
