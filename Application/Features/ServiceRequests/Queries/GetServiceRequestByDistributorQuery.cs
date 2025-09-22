
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetServiceRequestByDistributorQuery : IRequest<IResponseWrapper>
    {
        public Guid DistributorId { get; set; }
    }

    public class GetServiceRequestByDistributorQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetServiceRequestByDistributorQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceRequestByDistributorQuery request, CancellationToken cancellationToken)
        {
            var serviceRequestInDb = (await ServiceRequestService.GetServiceRequestByDistAsync(request.DistributorId)).Adapt<List<ServiceRequestResponse>>();

            if (serviceRequestInDb is not null)
            {
                return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(data: serviceRequestInDb);
            }
            return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(message: "ServiceRequest does not exists.");
        }
    }
}
