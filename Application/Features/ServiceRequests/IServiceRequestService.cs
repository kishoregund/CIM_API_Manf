using Application.Features.Customers.Responses;
using Application.Features.ServiceRequests.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface IServiceRequestService
    {
        Task<ServiceRequestResponse> GetServiceRequestAsync(Guid id);
        Task<Domain.Entities.ServiceRequest> GetServiceRequestEntityAsync(Guid id);
        Task<List<Domain.Entities.ServiceRequest>> GetServiceRequestsAsync();
        Task<string> GetServiceRequestNoAsync();
        Task<bool> OnBehalfOfCheck(Guid createdBy);
        Task<List<ServiceRequestResponse>> GetDetailServiceRequestsAsync(string businessUnitId, string brandId);
        Task<List<ServiceRequest>> GetDetailServiceRequestsOnlyAsync(string businessUnitId, string brandId);
        Task<List<ServiceRequestResponse>> GetServiceRequestByDistAsync(Guid distId);
        Task<SRInstrumentResponse> GetInstrumentDetailByInstrAsync(Guid instrumentId, Guid siteId);
        Task<Guid> CreateServiceRequestAsync(Domain.Entities.ServiceRequest serviceRequest);
        Task<Guid> UpdateServiceRequestAsync(Domain.Entities.ServiceRequest serviceRequest);
        Task<bool> DeleteServiceRequestAsync(Guid id);
        Task<List<ServiceRequestResponse>> GetServiceRequestBySRPIdAsync(Guid serviceReportId);
    }
}