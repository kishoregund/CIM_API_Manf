using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetServiceRequestNoQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetServiceRequestNoQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetServiceRequestNoQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceRequestNoQuery request, CancellationToken cancellationToken)
        {
            var ServiceRequestsInDb = await ServiceRequestService.GetServiceRequestNoAsync();

            if (!string.IsNullOrEmpty( ServiceRequestsInDb))
            {
                return await ResponseWrapper<string>.SuccessAsync(data: ServiceRequestsInDb);
            }
            return await ResponseWrapper<string>.SuccessAsync(message: "No Service Requests were found.");
        }
    }
}
