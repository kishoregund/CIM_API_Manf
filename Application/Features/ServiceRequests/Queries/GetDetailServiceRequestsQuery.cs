
using Application.Features.ServiceRequests.Responses;
using Application.Models;
using Domain.Entities;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetDetailServiceRequestsQuery : IRequest<IResponseWrapper>
    {
        public BUBrand BUBrand { get; set; }
    }

    public class GetDetailServiceRequestsQueryHandler(IServiceRequestService ServiceRequestService) : IRequestHandler<GetDetailServiceRequestsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDetailServiceRequestsQuery request, CancellationToken cancellationToken)
        {
            var serviceRequestInDb = await ServiceRequestService.GetDetailServiceRequestsAsync(request.BUBrand.BusinessUnitId, request.BUBrand.BrandId);

            if (serviceRequestInDb is not null)
            {
                return await ResponseWrapper<List<ServiceRequestResponse>>.SuccessAsync(data: serviceRequestInDb);
            }
            return await ResponseWrapper<ServiceRequestResponse>.SuccessAsync(message: "Service Request does not exists.");
        }
    }
}
