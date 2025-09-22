
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetServiceRequestBySRPQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetServiceRequestBySRPQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetServiceRequestBySRPQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetServiceRequestBySRPQuery request, CancellationToken cancellationToken)
        {
            var serviceRequestInDb = (await ServiceRequestService.GetServiceRequestBySRPIdAsync(request.ServiceReportId)).Adapt<List<ServiceRequestResponse>>();

            if (serviceRequestInDb is not null)
            {
                return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(data: serviceRequestInDb);
            }
            return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(message: "ServiceRequest does not exists.");
        }
    }
}
