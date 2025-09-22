using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetServiceRequestsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetServiceRequestsQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetServiceRequestsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceRequestsQuery request, CancellationToken cancellationToken)
        {
            var ServiceRequestsInDb = await ServiceRequestService.GetServiceRequestsAsync();

            if (ServiceRequestsInDb.Count > 0)
            {
                return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(data: ServiceRequestsInDb.Adapt<List<ServiceRequestResponse>>());
            }
            return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(message: "No ServiceRequests were found.");
        }
    }
}
