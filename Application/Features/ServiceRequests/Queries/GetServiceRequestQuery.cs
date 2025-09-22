
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetServiceRequestQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetServiceRequestQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetServiceRequestQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceRequestQuery request, CancellationToken cancellationToken)
        {
            var serviceRequestInDb = (await ServiceRequestService.GetServiceRequestAsync(request.ServiceRequestId)).Adapt<ServiceRequestResponse>();

            if (serviceRequestInDb is not null)
            {
                return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(data: serviceRequestInDb);
            }
            return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(message: "ServiceRequest does not exists.");
        }
    }
}
